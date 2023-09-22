using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BillMate.Data;
using BillMate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BillMate.Controllers
{
    [Route("api/[controller]")]
    public class StripeWebHook : Controller
    {
        private readonly BillMateDBContext _context;
        private static readonly string StripeAPIKey = "sk_test_51Lz1FrInaraDE4oEknHv9x2ExXBzgokC6SkxRZDU0ctg0lbFErKQ8SXfyO8sCgftsRk4BoykR7pJc28s8laWldEr00ABqiJMrc";

        public StripeWebHook(BillMateDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            try
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var stripeEvent = EventUtility.ParseEvent(json, false);

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;

                    if(!string.IsNullOrWhiteSpace(session.PaymentIntentId))
                    {
                        var service = new PaymentIntentService();
                        var paymentIntent = service.Get(session.PaymentIntentId, new PaymentIntentGetOptions(), new RequestOptions() { ApiKey = StripeAPIKey });

                        var paymentRequested = _context.PaymentRequested.Where(x => x.TransactionID == session.PaymentLinkId).FirstOrDefault();

                        if(paymentRequested != null)
                        {
                            var payment = new PaymentMade()
                            {
                                ClientId = paymentRequested.ClientId,
                                PatientId = paymentRequested.PatientId,
                                Amount = paymentIntent.AmountReceived / 100,
                                DepositDaeTime = DateTime.UtcNow,
                                NumberOfAttempts = 1,
                                TransactionID = paymentIntent.Id,
                                PaymentMethod = "PaymentLink",
                                BillingType = null
                            };

                            _context.Add(payment);

                            _context.PaymentRequested.Remove(paymentRequested);
                            _context.Entry(paymentRequested).State = EntityState.Deleted;

                            _context.SaveChanges();

                            return Ok();
                        }

                        return Ok();
                    }

                    return Ok();
                    // Then define and call a method to handle the successful attachment of a PaymentMethod.
                    // handlePaymentMethodAttached(paymentMethod);
                }
                // ... handle other event types
                else
                {
                    // Unexpected event type
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}

