using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillMate.Data;
using BillMate.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BillMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public InvoiceController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/Invoice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices([FromQuery(Name = "userId")] int userId)
        {
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

            var invoices = await _context.Invoices
                                    .Include(c => c.Client)
                                    .Include(a => a.Services)
                                    .ThenInclude(a => a.Service)
                                    .AsNoTracking()
                                    .Where(c => c.UserId == userId && c.CompanyId == companyId)
                                    .ToListAsync();
            return invoices;
        }

        // GET: api/Invoice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            try
            {
                var invoice = await _context.Invoices
                                        .Include(c => c.Client)
                                            .ThenInclude(c => c.AddressDetails)
                                        .Include(c => c.Client)
                                            .ThenInclude(c => c.BillingPreferences)
                                        .Include(a => a.Services)
                                            .ThenInclude(a => a.Service)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(c => c.Id == id);

                if (invoice == null)
                {
                    return NotFound();
                }
                return invoice;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return BadRequest(new { message = ex.InnerException.Message });
            }
        }

        // GET: api/Invoice/totalAmount
        [HttpPost("totalAmount")]
        public ActionResult<decimal> GetInvoiceAmount([FromBody] GetTotalAmountRequest requestData)
        {
            try
            {
                return GetInvoiceTotalAmount(requestData.StartTime, requestData.EndTime, requestData.ServiceIds, requestData.ClientId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return BadRequest(new { message = ex.InnerException.Message });
            }
        }

        // PUT: api/Invoice/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            var services = invoice.Services;

            var includeServiceIds = invoice.Services.Select(s => s.ServiceId).ToList();

            var totalAmount = GetInvoiceTotalAmount(invoice.StartTime, invoice.EndTime, includeServiceIds, invoice.ClientId);
            invoice.Amount = totalAmount ?? 0;

            _context.Entry(invoice).State = EntityState.Modified;

            _context.Database.ExecuteSqlRaw($@"DELETE FROM InvoiceServices WHERE InvoiceId={id}");

            foreach (var item in services)
            {
                item.Amount = GetInvoiceTotalAmountForService(invoice.StartTime, invoice.EndTime, item.ServiceId, invoice.ClientId) ?? 0;
                _context.Database.ExecuteSqlRaw($@"INSERT INTO InvoiceServices(InvoiceId,ServiceId,Amount, StoredTime) VALUES({item.InvoiceId}, {item.ServiceId}, {item.Amount}, GETDATE())");

            }

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

        // POST: api/Invoice
        // To protect from oveposting attacks, please enable the specific properties to bind to, for
        // more details if you want see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            try
            {
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

                var includeServiceIds = invoice.Services.Select(s => s.ServiceId).ToList();

                var totalAmount = GetInvoiceTotalAmount(invoice.StartTime, invoice.EndTime, includeServiceIds, invoice.ClientId);

                invoice.Amount = totalAmount ?? 0;
                invoice.Status = InvoiceStatus.Draft;
                invoice.CompanyId = companyId.Value;
                //invoice.StoredTime = DateTime.Now;

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                var invoiceServices = new List<InvoiceService>();

                foreach (var item in invoice.Services)
                {
                    var invoiceService = new InvoiceService()
                    {
                        InvoiceId = invoice.Id,
                        ServiceId = item.ServiceId,
                        Amount = GetInvoiceTotalAmountForService(invoice.StartTime, invoice.EndTime, item.ServiceId, invoice.ClientId) ?? 0
                    };

                    invoiceServices.Add(invoiceService);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { message = ex });
            }
        }

        // DELETE: api/Invoice/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Invoice>> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            var invoiceServices = _context.InvoiceServices.Where(x => x.InvoiceId == invoice.Id).ToList();

            _context.InvoiceServices.RemoveRange(invoiceServices);
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        private static decimal? GetInvoiceTotalAmount(long startTime, long endTime, List<int> serviceIds, int clientId)
        {
            try
            {
                decimal? totalAmount = 0;
                foreach (var item in serviceIds)
                {
                    var amount = GetInvoiceTotalAmountForService(startTime, endTime, item, clientId);
                    totalAmount += amount;
                }
                return totalAmount;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return null;
            }
        }

        private static decimal? GetInvoiceTotalAmountForService(long startTime, long endTime, int serviceId, int clientId)
        {
            try
            {
                using(var context = new BillMateDBContext())
                {
                    var service = context.Service.FirstOrDefault(x => x.Id == serviceId);

                    if (service == null)
                    {
                        return 0;
                    }

                    var startMonth = DateTimeOffset.FromUnixTimeSeconds(startTime).Month;
                    var endMonth = DateTimeOffset.FromUnixTimeSeconds(endTime).Month;

                    var months = 1;

                    if (endMonth - startMonth > 1)
                        months = endMonth - startMonth;

                    if (service.IsFlatFee && service.IsFeeSingleValue)
                    {
                        return service.FeeFlatValue * months;
                    }

                    if (service.CostMetric == "Total Production")
                    {
                        var totalProduction = context.DentalTotalProduction.FirstOrDefault(x => x.ClientId == clientId);

                        if(totalProduction == null)
                        {
                            return 0;
                        }

                        var amount = GetCostBasedOnMetrics(totalProduction.TotIncome, service.IsFlatFee, service.IsFeeSingleValue, service.FeeRanges, service.FeeFlatValue, service.FeePercentage);
                        return amount * months;
                    }
                    else if (service.CostMetric == "Total Collections")
                    {
                        var claims = context.DentalTotalClaims.Where(x => x.ClientId == clientId).ToList();

                        if (claims == null)
                        {
                            return 0;
                        }

                        var totalClaimsAmount = claims.Select(x => x.ClaimFees).Sum();

                        var amount = GetCostBasedOnMetrics(totalClaimsAmount, service.IsFlatFee, service.IsFeeSingleValue, service.FeeRanges, service.FeeFlatValue, service.FeePercentage);

                        return amount * months;
                    }
                    else if (service.CostMetric == "Total Claims Submitted")
                    {
                        var claims = context.DentalTotalClaims.Where(x => x.ClientId == clientId).ToList();

                        if (claims == null)
                        {
                            return 0;
                        }

                        var totalClaimsAmount = claims.Select(x => x.ClaimFees).Sum();

                        var amount = GetCostBasedOnMetrics(totalClaimsAmount, service.IsFlatFee, service.IsFeeSingleValue, service.FeeRanges, service.FeeFlatValue, service.FeePercentage);

                        return amount * months;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return null;
            }
        }

        private static decimal? GetCostBasedOnMetrics(decimal totalAmount, bool isFlatFee, bool singleValue, string feeRanges, decimal flatFee, decimal percentage)
        {
            try
            {
                var feeRangesConverted = JsonConvert.DeserializeObject<List<FeeRangeDTO>>(feeRanges);

                //TODO: if flatfee and range -> return flatFee because the range doesn't define a fee at the moment
                if (isFlatFee && !singleValue)
                {
                    return flatFee;
                }
                // if percentage and single value -> return percentage of the totalAmount provided
                else if (!isFlatFee && singleValue)
                {
                    return (totalAmount * percentage) / 100;
                }
                //TODO: if percentage and range -> return percentage of totalAmount because the range doesn't define a percentage value at the moment
                else if (!isFlatFee && !singleValue)
                {
                    return (totalAmount * percentage) / 100;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return null;
            }
        }

        public class GetTotalAmountRequest
        {
            public long StartTime { get; set; }
            public long EndTime { get; set; }
            public List<int> ServiceIds { get; set; }
            public int ClientId { get; set; }
        }

        public class FeeRangeDTO
        {
            public int FeeRangeValue1 { get; set; }
            public int FeeRangeValue2 { get; set; }
        }
    }
}
