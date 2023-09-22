//using BillMate.Controllers;
using BillMate.Data;
using BillMate.Emails;
using BillMate.Models;
using BillMate.Models.UrlShortener;
using BillMate.Services;
using BillMate.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillMate.Helpers
{
    public class SchedulerConfigurationHandler
    {
        private BillMateDBContext _context;
        private readonly EmailService _emailService;
        private readonly UrlShortnerAPICaller _urlShortnerAPICaller;

        public SchedulerConfigurationHandler(BillMateDBContext context, EmailService emailService,
           UrlShortnerAPICaller urlShortnerAPICaller)
        {
            _context = context;
            _emailService = emailService;
            _urlShortnerAPICaller = urlShortnerAPICaller;
        }

        public SchedulerConfigurationHandler()
        {

        }

        private DentalPatient GetDentalPatient(PaymentRequested paymentRequested)
        {
            return _context.DentalPatient.Where(x => x.PatientId == paymentRequested.PatientId && x.ClientId == paymentRequested.ClientId).FirstOrDefault();
        }

        private PatientPayment GetPatientPayment(PaymentRequested paymentRequested)
        {
            return _context.PatientPayment.Where(x => x.PatientId == paymentRequested.PatientId && x.ClientId == paymentRequested.ClientId).FirstOrDefault();
        }


        private void ProcessConfig(SchedulerConfiguration config, PatientPayment patient = null, DentalPatient dentalPatient = null, PaymentLink paymentLink = null)
        {
            if (config != null)
            {
                var paymentRequestedList = _context.PaymentRequested.Where(x => x.ClientId == config.ClientId).ToList();
              
                foreach (var paymentRequested in paymentRequestedList)
                {
                    dentalPatient = GetDentalPatient(paymentRequested);
                    patient = patient ?? GetPatientPayment(paymentRequested);
                    // if DentalPatient and PaymentLink are null, get them from the table, otherwise use provided parameters
                    // Get the last attempt number and date for this patient
                    var lastAttempt = _context.PatientPaymentAttempt
                        .Where(a => a.PatientId == dentalPatient.PatientId && a.ClientId == dentalPatient.ClientId)
                        .OrderByDescending(a => a.AttemptNumber)
                        .FirstOrDefault();

                   
                    if (lastAttempt != null)
                    {
                        // if the current attempt number is equal to the last attempt number + 1
                        // and the number of days since the last attempt is greater than or equal to config.SendAfterDays
                        // then send the communication
                        if (config.AttemptNumber == lastAttempt.AttemptNumber + 1 &&
                            (DateTime.UtcNow - lastAttempt.DateAttempted).TotalDays >= config.SendAfterDays)
                        {
                            SendCommunication(config, ref dentalPatient, ref paymentLink, paymentRequested);
                        }
                    }
                    else
                    {
                        // If there's no last attempt (i.e., this is the first attempt), send the communication
                        SendCommunication(config, ref dentalPatient, ref paymentLink, paymentRequested);
                    }
                }
            }
        }

        void SendCommunication(SchedulerConfiguration config, ref DentalPatient dentalPatient, ref PaymentLink paymentLink, PaymentRequested paymentRequested, PatientPayment patient = null)
        {
            dentalPatient = dentalPatient ?? GetDentalPatient(paymentRequested);
            paymentLink = paymentLink ?? GeneratePaymentLink(paymentRequested);
            DateTime attemptDateTime = DateTime.UtcNow;

            int clientId = dentalPatient.ClientId;
            int patientId = dentalPatient.PatientId;

            string amountTotalDue = patient == null ? paymentRequested.TotalDue.ToString() : patient.EstPatientDue;


            var clientDetails = GetClientDetails(dentalPatient, paymentRequested, paymentLink, config, amountTotalDue, dentalPatient.FirstName);

            var existingAttempt = _context.PatientPaymentAttempt
            .Where(x => x.PatientId == patientId && x.ClientId == clientId)
            .OrderByDescending(x => x.DateAttempted)
            .FirstOrDefault();

            //if the existing attempt is null, the attempt number is 1, else increase the existing attempt by 1
            int attemptNumber = existingAttempt != null ? existingAttempt.AttemptNumber + 1 : 1;
            if (ShouldSendEmail(config, dentalPatient))
            {
                string messageStatus = _emailService.SendEmail(clientDetails, config.EmailMessage) ? "Sent" : "Failed";
                
                LogAttempt(messageStatus, dentalPatient, "email", config.EmailMessage, attemptDateTime, attemptNumber);
            }
            else
            {
                //todo log in database that we can't send the email because we don't have an email address
            }

            if (ShouldSendText(config, dentalPatient))
            {
                string messageStatus = SendTextMessage(dentalPatient, config, paymentLink, amountTotalDue,
                    dentalPatient.FirstName, clientDetails.clientDetail.PracticeName) ? "Sent" : "Failed";
                LogAttempt(messageStatus, dentalPatient, "text", config.EmailMessage, attemptDateTime, attemptNumber);
            }
            else
            {
                //todo log in database that we can't send the text because we don't have a phone number
                //need to add reason for the failure
            }
            
            AddOrUpdatePaymentRequest(paymentRequested, paymentLink, dentalPatient, patient);
        }

        private void LogAttempt(string status, DentalPatient patient, string notificateType, string message, DateTime attemptDateTime, int attemptNumber)
        {
            var currentAttempt = new PatientPaymentAttempt
            {
                Status = status,
                DateAttempted = attemptDateTime,
                NotificationType = notificateType,
                Message = message,
                ClientId = patient.ClientId,
                PatientId = patient.PatientId,
                AttemptNumber = attemptNumber,
                StoredTime = DateTime.UtcNow
            };

            _context.PatientPaymentAttempt.Add(currentAttempt);
            _context.SaveChanges();

        }

        private void AddOrUpdatePaymentRequest(PaymentRequested initialPaymentRequested, PaymentLink paymentLink, DentalPatient dentalPatient, PatientPayment patient)
        {
            try
            {
                string amountTotalDue = patient == null ? initialPaymentRequested.TotalDue.ToString() : patient.EstPatientDue;
                decimal totalDue = Convert.ToDecimal(amountTotalDue);
                var existingPaymentRequest = _context.PaymentRequested.Where(x => x.PatientId == dentalPatient.PatientId && x.ClientId == dentalPatient.ClientId && x.TotalDue == totalDue).FirstOrDefault();
                
                //this is the first time requesting payment
                //patient should not be null
                if (existingPaymentRequest == null)
                {
                    var paymentRequested = new PaymentRequested()
                    {
                        ClientId = dentalPatient.ClientId,
                        SentDateTime = DateTime.UtcNow,
                        TotalDue = totalDue,
                        AmountPaid = null,
                        NumberOfAttempts = paymentLink == null ? 0 : 1,
                        FutureVisitDate = dentalPatient.DateFutureVisit,
                        TransactionID = paymentLink == null ? null : paymentLink.Id,
                        PatientId = dentalPatient.PatientId,
                    };

                    _context.Add(paymentRequested);
                    _context.SaveChanges();
                    RemovePatientFromPatientPaymentTable(patient);
                }
                else
                {
                    existingPaymentRequest.NumberOfAttempts = existingPaymentRequest.NumberOfAttempts + 1;
                    existingPaymentRequest.SentDateTime = DateTime.UtcNow;
                    _context.Entry(existingPaymentRequest).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        private void RemovePatientFromPatientPaymentTable(PatientPayment patient)
        {
            _context.PatientPayment.Remove(patient);
            _context.Entry(patient).State = EntityState.Deleted;
            _context.SaveChanges();
        }


        private PaymentLink GeneratePaymentLink(PaymentRequested paymentRequested)
        {
            return StripeService.GenerateStripePaymentLink(Convert.ToDecimal(paymentRequested.TotalDue));
        }

        private bool ShouldSendEmail(SchedulerConfiguration config, DentalPatient dentalPatient)
        {
            return !string.IsNullOrWhiteSpace(dentalPatient.Email) && config.IsEmailNotificationEnabled == true;
        }

        private GeneralPaymentLink GetClientDetails(DentalPatient dentalPatient, PaymentRequested paymentRequested, PaymentLink paymentLink, SchedulerConfiguration config, string totalDue, string patientFirstName)
        {
            var clientId = dentalPatient != null ? dentalPatient.ClientId : paymentRequested.ClientId;
            var client = _context.Client.Where(x => x.Id == clientId).FirstOrDefault();
            var clientDetails = _context.ClientDetails.Where(x => x.ClientId == clientId).FirstOrDefault();
            return new GeneralPaymentLink(client, clientDetails, client.Id, dentalPatient != null ? dentalPatient.Email : null, paymentLink.Url, totalDue, patientFirstName);
        }


        private bool ShouldSendText(SchedulerConfiguration config, DentalPatient dentalPatient)
        {
            //check if the wireless phone number is empty, if it is empty, check the home phone number, if it is empty chceck the workphone
            var phone = !string.IsNullOrWhiteSpace(dentalPatient.WirelessPhone)
            ? dentalPatient.WirelessPhone
            : !string.IsNullOrWhiteSpace(dentalPatient.HomePhone)
              ? dentalPatient.HomePhone
              : dentalPatient.WorkPhone;

            return !string.IsNullOrWhiteSpace(phone) && config.IsTextNotificationEnabled == true;
        }

        private bool SendTextMessage(DentalPatient dentalPatient, SchedulerConfiguration config, PaymentLink paymentLink, string totalDue, 
            string patientFirstName, string practiceName)
        {
            var phone = dentalPatient.WirelessPhone ?? dentalPatient.WorkPhone ?? dentalPatient.HomePhone;
            var smsText = paymentLink.Url;
            try
            {
                UrlParameter urlParameter = new UrlParameter
                {
                    ClientId = dentalPatient.ClientId,
                    PatientId = dentalPatient.PatientId,
                    URL = smsText
                };
                var apiResponse = _urlShortnerAPICaller.ShortenUrl(urlParameter).GetAwaiter().GetResult();
                if(apiResponse != null)
                {
                    smsText = apiResponse.Data.ToString();
                }
            }
            catch (Exception ex)
            {
                //todo log that you can't shorten url
            }
            
            config.TextMessage = config.TextMessage.Replace("{PracticeName}", practiceName).Replace("${TotalDue}", $"{Convert.ToDecimal(totalDue):#0.00}").Replace(patientFirstName, patientFirstName);
            return TwilioService.SendSMS(phone, config.TextMessage + " " + smsText);
        }

        public void SendSubsequentAttempts(List<SchedulerConfiguration> configs)
        {
            foreach (var config in configs)
            {
                ProcessConfig(config);
            }
        }
        public void SendFirstAttempt(PatientPayment patient, DentalPatient dentalPatient, PaymentLink paymentLink,
           SchedulerConfiguration config)
        {
            if(config != null)
            {
                SendCommunication(config, ref dentalPatient, ref paymentLink, null, patient);
            }
        }
    }
}
