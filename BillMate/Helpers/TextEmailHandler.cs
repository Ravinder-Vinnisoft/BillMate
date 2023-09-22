using BillMate.Emails;
using BillMate.Models;
using BillMate.Services;
using Stripe;
using Twilio.TwiML.Voice;
using Client = BillMate.Models.Client;

namespace BillMate.Helpers
{
    public class TextEmailHandler
    {
        public void SendCommunication(Client dentalPractice, ClientDetail dentalPracticeDetail, string patientEmail,
            string patientPhoneNumber, PaymentLink paymentLink, bool shouldSendEmail, bool shouldSendText,
            string emailBody, string textBody)
        {
            if(shouldSendEmail)
            { 
                if(!string.IsNullOrEmpty(patientEmail))
                {
                  //  var email = new GeneralPaymentLink(dentalPractice, dentalPracticeDetail, dentalPractice.Id, patientEmail, paymentLink.Url);
                    //EmailService.SendEmail(email, emailBody);
                }
            }

            if(shouldSendText)
            {
                if(!string.IsNullOrWhiteSpace(patientPhoneNumber))
                {
                    var smsText = $"Please click on the link to process the payment - {paymentLink.Url}";
                   // TwilioService.SendSMS(patientPhoneNumber, textBody + "    " + smsText);
                }
            }
        }
    }
}
