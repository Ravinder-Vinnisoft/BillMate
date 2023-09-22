using BillMate.Data;
using BillMate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientServiceController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public ClientServiceController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/ClientService/GetServices
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices(int? clientId = null)
        {
            try {

                var user = HttpContext.User;
                string companyIdString = null;
                int? companyId = null;
                if (user != null)
                {
                    if (user.HasClaim(x => x.Type == "CompanyId"))
                    {
                        companyIdString = user.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value;
                        companyId = Convert.ToInt32(companyIdString);
                    }
                }
                else
                {
                    throw new Exception("User is not linked to a company");
                }

                if (clientId.HasValue)
                {
                    var clientServices = new List<Service>();
                    var services = await _context.Service.AsNoTracking().Where(x => x.CompanyId == companyId).ToListAsync();

                    foreach (var item in services)  
                    {
                        var assignedClients = item.AssignedClients ?? null;
                        if(!string.IsNullOrWhiteSpace(assignedClients))
                        {
                            var clients = assignedClients.Split(',');
                            if(clients.Contains(clientId.ToString()))
                            {
                                clientServices.Add(item);
                                continue;
                            }
                        }
                    }
                    return clientServices;
                }
                return await _context.Service.Where(x => x.CompanyId == companyId).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.InnerException.Message });
            }
        }

        // GET: api/ClientService/5
        [HttpGet("[action]")]
        public async Task<ActionResult<Service>> GetService([FromQuery] int id)
        {
            try
            {
                var service = await _context.Service.AsNoTracking().FirstAsync(c => c.Id == id);

                if (service == null)
                {
                    return NotFound();
                }
                return service;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/ClientService
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            try
            {
                var user = HttpContext.User;
                string companyIdString = null;
                int? companyId = null;
                if (user != null)
                {
                    if (user.HasClaim(x => x.Type == "CompanyId"))
                    {
                        companyIdString = user.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value;
                        companyId = Convert.ToInt32(companyIdString);
                    }
                }
                else
                {
                    throw new Exception("User is not linked to a company");
                }

                service.CompanyId = companyId.Value;
                _context.Service.Add(service);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetServices", service);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { message = ex });
            }
        }


        // PUT: api/ClientService/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
            if (id != service.Id)
            {
                return BadRequest();
            }

            var requestUser = HttpContext.User;
            string companyIdString = null;
            int? companyId = null;
            if (requestUser != null)
            {
                if (requestUser.HasClaim(x => x.Type == "CompanyId"))
                {
                    companyIdString = requestUser.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value;
                    companyId = Convert.ToInt32(companyIdString);
                }
            }
            else
            {
                throw new Exception("User is not linked to a company");
            }

            service.CompanyId = companyId;
            _context.Entry(service).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            return NoContent();
        }

        // DELETE: api/ClientService/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Service>> DeleteService(int id)
        {
            var service = await _context.Service.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            _context.Service.Remove(service);

            await _context.SaveChangesAsync();

            return service;
        }

        // GET: api/ClientService/GetStripeConnectionStatus
        [HttpGet("[action]")]
        public async Task<ActionResult<bool>> GetStripeConnectionStatus([FromQuery] int userId)
        {
            try
            {
                var user = await _context.User.AsTracking().FirstAsync(c => c.Id == userId);
                string stripeId;
                if (user == null)
                {
                    return NotFound();
                }
                // if new client
                if (user.StripeId == null)
                {
                    stripeId = CreateClientStripeAccount(user);
                    user.StripeId = stripeId;
                    await this.UpdateClientStripeId(user);
                }
                return AuthWithStripe(user.StripeId);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private async Task<IActionResult> UpdateClientStripeId(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { message = ex.Message });
            }
        }

        private string CreateClientStripeAccount(User user)
        {
            //TODO: Set and retrieve from environment variable
            StripeConfiguration.ApiKey = "sk_test_51KXZdLAYIx7zyxW0AJZN7XWkHv35BMNpuDJx77L1LE7koA6WizCTrNKw1jQtHtaryREo687kR7Yiy06TyWQnIUEI00fPeVCIVF";
            var options = new AccountCreateOptions
            {
                Type = "custom",
                Country = "US",
                Email = user.EmailId,
                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions
                    {
                        Requested = true,
                    },
                    Transfers = new AccountCapabilitiesTransfersOptions
                    {
                        Requested = true,
                    },
                },
            };
            var service = new AccountService();
            Account account = service.Create(options);
            return account.Id;
        }

        private bool AuthWithStripe(string stripeId)
        {
            //TODO: Set and retrieve from environment variable
            StripeConfiguration.ApiKey = "sk_test_51KXZdLAYIx7zyxW0AJZN7XWkHv35BMNpuDJx77L1LE7koA6WizCTrNKw1jQtHtaryREo687kR7Yiy06TyWQnIUEI00fPeVCIVF";

            var options = new AccountLinkCreateOptions
            {
                Account = stripeId,
                RefreshUrl = "https://localhost:44323/client-services",
                ReturnUrl = "https://localhost:44323/client-services",
                Type = "account_onboarding",
                Collect = "eventually_due",
            };
            var service = new AccountLinkService();
            var account = service.Create(options);
            return account.StripeResponse.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
