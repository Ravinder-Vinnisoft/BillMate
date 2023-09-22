using System;
using BillMate.Models;
using BillMate.Services;
using BillMate.Services.Abstractions;

namespace BillMate.Emails
{
    public class GeneralPaymentLink : Email
    {
        public Client client { get; set; }
        public ClientDetail clientDetail { get; set; }
        public int clientId { get; set; }
        public string paymentLink { get; set; }

        public override bool Bcc => throw new NotImplementedException();

        public string emailId = string.Empty;

        public string TotalDue { get; set; }

        public string PatientFirstName { get; set; }

        public GeneralPaymentLink(Client c, ClientDetail cd, int clientId, string email, string pl, string totalDue, 
            string patientFirstName) : base(c)
        {
            client = c;
            this.clientId = clientId;
            clientDetail = cd;
            emailId = email;
            paymentLink = pl;
            TotalDue = totalDue;
            PatientFirstName = patientFirstName;
        }

        public override string TemplateName()
        {
            return "PaymentLink";
        }

        public override int ClientId()
        {
            return client.Id;
        }

        public override string SendTo()
        {
            return emailId;
        }

        public override string SendToName()
        {
            return null;
        }

        public override string Subject()
        {
            return string.Format("Payment Link");
        }

        public string PaymentLink()
        {
            return paymentLink;
        }
    }
}
