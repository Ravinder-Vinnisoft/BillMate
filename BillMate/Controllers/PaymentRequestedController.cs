using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillMate.Data;
using BillMate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BillMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentRequestedController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public PaymentRequestedController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/PaymentRequested
        [HttpGet]
        public ActionResult<IEnumerable<PaymentRequested>> GetPaymentsRequested(int clientId)
        {
            try
            {
                var payments = _context.PaymentRequested.Where(x => x.ClientId == clientId).ToList();

                foreach (var payment in payments)
                {
                    payment.Patient = _context.DentalPatient.Where(x => x.PatientId == payment.PatientId).FirstOrDefault();
                }

                return payments;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}

