using BillMate.Data;
using BillMate.Emails;
using BillMate.Helpers;
using BillMate.Services.Interface;
using System.Collections.Concurrent;
using System.IO;
using System;
using BillMate.Models;
using System.Linq;

namespace BillMate.Services.Abstractions
{
    public abstract class Email
    {
        public static ConcurrentDictionary<string, string> DomainMapping = new ConcurrentDictionary<string, string>();
        public abstract bool Bcc { get; }
        public abstract int ClientId();
        public abstract string TemplateName();
        public abstract string Subject();
        public abstract string SendTo();
        public abstract string SendToName();
        public virtual int GetAttachmentStatus() { return 0; }
        public virtual FileInfo GetAttachment(int no) { throw new Exception(); }
        private Client c;


        public Email(Client client)
        {
      
        }

        public string GetClientImageUrl()
        {
            var clientId = ClientId();
            string result = "";
            if (DomainMapping.ContainsKey(clientId.ToString()))
            {
                if (DomainMapping.TryGetValue(clientId.ToString(), out result))
                {
                    return result;
                }
            }
            using (var context = new BillMateDBContext())
            {
                string clientURL = context.ClientDetails.Where(x => x.ClientId == clientId).Select(x => x.LogoURL).FirstOrDefault();

                if (string.IsNullOrEmpty(clientURL))
                {
                    return null;
                }

                result = clientURL;

                DomainMapping.TryAdd(clientId.ToString(), result);

                return result;
            }
        }

        public string PrepareBodyAsync(GeneralPaymentLink email, string customText, IIMageService _imageService)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "PaymentLink.html");
                string content = File.ReadAllText(path);

                string clientName = email.clientDetail.PracticeName;
                string paymentLink = email.paymentLink;
                string logo = _imageService.SaveBase64AsImage(email.GetClientImageUrl(), email.clientDetail.ClientId.ToString());
                
                string html = content.Replace("{practiceName}", clientName).
                                      Replace("{paymentLink}", paymentLink).
                                      Replace("{logo}", logo).
                                      Replace("{customText}", customText).
                                      Replace("{practiceAddress}", email.clientDetail.PracticeAddress).
                                      Replace("{practicePhoneNumber}", PhoneNumberFormatter.FormatPhoneNumber(email.clientDetail.PracticePhoneNumber)).
                                      Replace("{PracticeName}", clientName).
                                      Replace("{PatientFirstName}", email.PatientFirstName).
                                      Replace("{TotalDue}", $"${Convert.ToDecimal(email.TotalDue):#0.00}").
                                      Replace("{PracticePhoneNumber}", PhoneNumberFormatter.FormatPhoneNumber(email.clientDetail.PracticePhoneNumber));


                return html;
            }
            catch (Exception exc)
            {
               

                throw exc;
            }
        }
    }
}
