using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using BillMate.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using BillMate.Helpers;
using BillMate.Services;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Hangfire;
using Hangfire.SqlServer;
using System;
using BillMate.Services.Interface;

namespace BillMate
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime.Then Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //not using this
            //services.AddControllers().AddNewtonsoftJson();
            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.Converters.Add(new ByteArrayConverter());
            //});

            //    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            //.AddNewtonsoftJson(opt => {
            //    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //});

            services.AddMvc(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                foreach (var formatter in options.InputFormatters)
                {
                    if (formatter.GetType() == typeof(SystemTextJsonInputFormatter))
                        ((SystemTextJsonInputFormatter)formatter).SupportedMediaTypes.Add(
                        Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/plain"));
                }
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddCors();
            //services.AddControllers();

            // configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = CompressionLevel.Fastest
            );
            services.AddDbContext<BillMateDBContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("TestsContext")));

            // Add Hangfire services.
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.FromSeconds(50),
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            services.AddSingleton<IHangfireRecurringJob, HangfireRecurringJobs>();
            services.AddSingleton<IIMageService, ImageService>();
            services.AddSingleton<UrlShortnerAPICaller>();
            services.AddScoped<EmailService>();
            services.AddScoped<SchedulerConfigurationHandler>();
            services.AddSingleton<IURLShortnerService, URLShortnerService>();
        }

        // This method gets called by runtime. Use method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IRecurringJobManager recurringJobManager, IServiceProvider serviceProvider, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHangfireDashboard();
            recurringJobManager.AddOrUpdate("Run every 6 hours", () => serviceProvider.GetService<IHangfireRecurringJob>().SendPaymentlinktoPatients(), "0 */6 * * *");

            app.UseHttpsRedirection();

            // this line enables static file serving from wwwroot
            app.UseStaticFiles(); 

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                                Path.Combine(env.ContentRootPath, "ClientApp/src/assets")),
                RequestPath = "/StaticWebFiles"
            });

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapHangfireDashboard();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
