using System;
using System.Linq;
using BillMate.Data;
using BillMate.Emails;
using BillMate.Helpers;
using BillMate.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace BillMate.Services
{
    public class HangfireRecurringJobs : IHangfireRecurringJob
    {
        private readonly BillMateDBContext _context;
        private readonly SchedulerConfigurationHandler _schedulerConfigurationHandler;
        public HangfireRecurringJobs(BillMateDBContext context, SchedulerConfigurationHandler schedulerConfigurationHandler)
        {
            _context = context;
            _schedulerConfigurationHandler = schedulerConfigurationHandler;
        }

        public void SendPaymentlinktoPatients()
        {
            //get the second through n attempts
            var configs = _context.SchedulerConfiguration.Where(s => s.AttemptNumber != 1).ToList();
          //  SchedulerConfigurationHandler schedulerConfigurationHandler = new SchedulerConfigurationHandler(_context);
            _schedulerConfigurationHandler.SendSubsequentAttempts(configs);
            //foreach (var config in configs)
            //{
            //    if (config != null && config.StoredTime.AddDays(Convert.ToDouble(config.SendAfterDays.Value)) > DateTime.UtcNow)
            //    {
            //        var paymentRequested = _context.PaymentRequested.Where(x => x.ClientId == config.ClientId).ToList();

            //        foreach (var patientInPaymentsRequestedTable in paymentRequested)
            //        {
            //            var dentalPatient = _context.DentalPatient.Where(x => x.PatientId == patientInPaymentsRequestedTable.PatientId).FirstOrDefault();

            //            var paymentLink = StripeService.GenerateStripePaymentLink(Convert.ToDecimal(patientInPaymentsRequestedTable.TotalDue));

            //            if (!string.IsNullOrWhiteSpace(dentalPatient.Email) && config.IsEmailNotificationEnabled.HasValue && config.IsEmailNotificationEnabled.Value)
            //            {

            //                var client = _context.Client.Where(x => x.Id == patientInPaymentsRequestedTable.ClientId).FirstOrDefault();
            //                var clientDetails = _context.ClientDetails.Where(x => x.ClientId == patientInPaymentsRequestedTable.ClientId).FirstOrDefault();
            //                var email = new GeneralPaymentLink(client, clientDetails, client.Id, dentalPatient.Email, paymentLink.Url);

            //                EmailService.SendEmail(email, config.EmailMessage);
            //            }

            //            var phone = dentalPatient.WorkPhone ?? dentalPatient.HomePhone;
            //            if (!string.IsNullOrWhiteSpace(phone) && config.IsTextNotificationEnabled.HasValue && config.IsTextNotificationEnabled.Value)
            //            {
            //                var smsText = $"Please click on the link to process the payment - {paymentLink.Url}";
            //                TwilioService.SendSMS(phone, config.TextMessage + "    " + smsText);
            //            }

            //            patientInPaymentsRequestedTable.NumberOfAttempts += 1;
            //            patientInPaymentsRequestedTable.TransactionID = paymentLink.Id;
            //            patientInPaymentsRequestedTable.SentDateTime = DateTime.UtcNow;
            //            _context.Entry(patientInPaymentsRequestedTable).State = EntityState.Modified;
            //            _context.SaveChanges();
            //        }
            //    }
            //}
        }
    }

    public interface IHangfireRecurringJob
    {
        void SendPaymentlinktoPatients();
    }
}
