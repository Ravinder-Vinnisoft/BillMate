using BillMate.Data;
using BillMate.Helpers;
using BillMate.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly BillMateDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CompanyController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies.Include(a => a.AddressDetails).ToListAsync();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            try
            {
                var company = await _context.Companies.Include(a => a.AddressDetails).AsNoTracking().FirstAsync(c => c.Id == id);

                if (company == null)
                {
                    return NotFound();
                }
                return company;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return BadRequest(new { message = ex.InnerException.Message });
            }
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;
            _context.Entry(company.AddressDetails).State = EntityState.Modified;

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

        // POST: api/Companies
        // To protect from oveposting attacks, please enable the specific properties to bind to, for
        // more details if you want see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            try
            {
                _context.Companies.Add(company);
                await _context.SaveChangesAsync();
                Console.WriteLine("Success");

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { message = ex });
            }
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            var address = await _context.AddressDetails.FindAsync(company.AddressId);
            if (address == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            _context.AddressDetails.Remove(address);
            
            await _context.SaveChangesAsync();

            return company;
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.Id == id);
        }
    }
}