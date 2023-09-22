using System;
using PostmarkDotNet;
using PostmarkDotNet.Model;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BillMate.Services
{
	public class TwilioService
	{
        //AC297116131e8196d4432d59a8278fada6
        public static string accountSid = "AC297116131e8196d4432d59a8278fada6";

        //ac477e3028eb4786d16f8d0b7f93cf98
        public static string authToken = "ac477e3028eb4786d16f8d0b7f93cf98";

        public static bool SendSMS(string phoneNumber, string text)
        {
            bool didSend = true;

            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(text))
                    return false;

                TwilioClient.Init(accountSid, authToken);
                var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber));

                //MGfab40837c187608afcb8a04865ae4827
                messageOptions.MessagingServiceSid = "MGfab40837c187608afcb8a04865ae4827";
                messageOptions.Body = text;

                var message = MessageResource.Create(messageOptions);
            }
            catch (Exception exc)
            {
                //todo log exception, can you find out why and the format
                didSend = false;
            }
            return didSend;
        }
    }
}

