using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net;
using System;
using BillMate.Models.UrlShortener;
using BillMate.Services.Interface;
using Microsoft.Extensions.Configuration;

namespace BillMate.Controllers
{
    public class RController : Controller
    {
        IURLShortnerService _urlShortnerService { get; set; }
        private readonly IConfiguration _configuration;

        public RController(IURLShortnerService urlShortnerService, IConfiguration configuration)
        {
            _urlShortnerService = urlShortnerService;
            _configuration = configuration;
        }

        [HttpGet("{r}/{token}")]
        public IActionResult Get(string token)
        {
            var url = _urlShortnerService.GetURLByToken(token);

            if (string.IsNullOrEmpty(url))
            {
                return Redirect("https://google.com");
            }

            return Redirect(url);
        }

        [HttpPost("api/shorten")]
        public IActionResult PostURL([FromBody] UrlParameter urlParameter)
        {
            var apiResponse = new APIResponse<object> { Data = new object() };

            try
            {
                if (!urlParameter.URL.Contains("https"))
                {
                    if (!urlParameter.URL.Contains("http"))
                    {
                        urlParameter.URL = "https://" + urlParameter.URL;
                    }
                }

                // Assuming Shortner is a service that shortens URLs

                Shortner shortURL = new Shortner(urlParameter.URL, urlParameter.ClientId, urlParameter.PatientId,
                    _urlShortnerService, _configuration);
                apiResponse.StatusCode = (int)HttpStatusCode.OK;
                apiResponse.Data = shortURL.Token;
            }
            catch (Exception ex)
            {
                apiResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " ---------" + ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        errorMessage += " ---------" + ex.InnerException.InnerException.Message;
                    }
                }

                apiResponse.Message = errorMessage;
            }

            return StatusCode(apiResponse.StatusCode, apiResponse);
        }

    }
}
