using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using BillMate.Data;
using BillMate.Emails;
using BillMate.Models;
using PostmarkDotNet;
using PostmarkDotNet.Model;
using Task = System.Threading.Tasks.Task;
using BillMate.Helpers;
using BillMate.Services.Interface;
using BillMate.Services.Abstractions;
using Microsoft.Extensions.Logging;
using NLog;

namespace BillMate.Services
{
	public class EmailService
	{
        private readonly IIMageService _imageService;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IIMageService imageService, ILogger<EmailService> logger)
        {
            _imageService = imageService;
            _logger = logger;

        }
        public static string POSTMARK_API_TOKEN = "82aa4f67-34d4-4861-8835-34ff0c6d9cc9";

        public bool SendEmail(Email email, string customText)
        {
            bool didSend = false;
            if (!string.IsNullOrWhiteSpace(email.SendTo()))
            {
                try
                {
                    var body = email.PrepareBodyAsync((GeneralPaymentLink)email, customText, _imageService);
                    var html = body;
                    SendEmailViaPostmark(email, html);
                    didSend = true;
                    _logger.LogInformation("did send email");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, "Send Email");
                    didSend = false;
                }
            }

            return didSend;
        }


        private  string GetReplyToAddress(GeneralPaymentLink email)
        {
           return email.clientDetail.ReplyTo;
        }
        public  void SendEmailViaPostmark(Email email, string body)
        {
            try
            {
                var message = new PostmarkMessage()
                {
                    To = email.SendTo(),
                    From = "noreply@getbillmate.com",
                    ReplyTo = GetReplyToAddress((GeneralPaymentLink)email),
                    TrackOpens = true,
                    Subject = email.Subject(),
                    HtmlBody = body,
                    TextBody = body,
                    Headers = new HeaderCollection {
                        new MailHeader("X-CUSTOM-HEADER", "Header content")
                    }
                };

                var client = new PostmarkClient(POSTMARK_API_TOKEN);
                var sendResult = Task.Run(async () => await client.SendMessageAsync(message));
                sendResult.Wait();

                if (sendResult.Result.Status != PostmarkStatus.Success) {
                    throw new Exception("There was an error whilst sending email, please try again");
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "SendEmailViaPostmark");
                throw exc;
            }
        }
    }


 
}

