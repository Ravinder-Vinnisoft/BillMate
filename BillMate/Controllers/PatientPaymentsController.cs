using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BillMate.Data;
using BillMate.Emails;
using BillMate.Helpers;
using BillMate.Models;
using BillMate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Twilio.TwiML.Voice;

namespace BillMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientPaymentsController : ControllerBase
    {
        private readonly BillMateDBContext _context;
        private readonly SchedulerConfigurationHandler _schedulerConfigurationHandler;


        public PatientPaymentsController(BillMateDBContext context, SchedulerConfigurationHandler schedulerConfigurationHandler)
        {
            _context = context;
            _schedulerConfigurationHandler = schedulerConfigurationHandler;
        }

        // GET: api/PatientPayments
        [HttpGet]
        public ActionResult<IEnumerable<PatientPayment>> GetPatientPayments(int clientId)
        {
            try
            {
                return _context.PatientPayment.Where(x => x.ClientId == clientId).ToList();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        // POST: api/PatientPayments/RequestPayment
        [HttpPost, Route("RequestPayment")]
        public ActionResult<dynamic> SendStripeEmailTextToPayment(string PatientIds, int clientId)
        {
            try
            {
                var patientIdsInt = PatientIds.Split(',').ToList();

                var dentalPatients = _context.PatientPayment.Where(x => patientIdsInt.Contains(x.PatientId.ToString())).ToList();

                foreach (var patient in dentalPatients)
                {
                    var dentalPatient = _context.DentalPatient.Where(x => x.PatientId == patient.PatientId && x.ClientId == clientId).FirstOrDefault();
                    var clientConfig = _context.SchedulerConfiguration.Where(x => x.ClientId == patient.ClientId && x.AttemptNumber == 1).FirstOrDefault();

                    if (clientConfig != null && clientConfig.SendAfterDays.Value == 0)
                    {
                        if (patient.EstPatientDue.StartsWith("$"))
                        {
                            patient.EstPatientDue = patient.EstPatientDue.Replace("$", "");
                        }
                        
                        var paymentLink = StripeService.GenerateStripePaymentLink(Convert.ToDecimal(patient.EstPatientDue));
                        var config = _context.SchedulerConfiguration.Where(s => s.ClientId == patient.ClientId && s.AttemptNumber == 1).FirstOrDefault();

                        _schedulerConfigurationHandler.SendFirstAttempt(patient, dentalPatient, paymentLink, config);
                    }
                    else
                    {
                        //addORUpdatePaymentRequestedDetails(patient, null);

                        //_context.PatientPayment.Remove(patient);
                        //_context.Entry(patient).State = EntityState.Deleted;
                        //_context.SaveChanges();
                    }
                }

                return Ok();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

    }
}
