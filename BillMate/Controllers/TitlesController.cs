using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BillMate.Data;
using BillMate.Models;

namespace BillMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitlesController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public TitlesController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/Titles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Title>>> GetTitles()
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

            return await _context.Titles.Where(x => x.CompanyId == companyId).ToListAsync();
        }

        // GET: api/Titles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Title>> GetTitle(int id)
        {
            var title = await _context.Titles.FindAsync(id);

            if (title == null)
            {
                return NotFound();
            }

            return title;
        }

        // PUT: api/Titles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTitle(int id, Title title)
        {
            if (id != title.Id)
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

            title.CompanyId = companyId;

            _context.Entry(title).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TitleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest(new { message = "Job Title is already present" });
                }
            }

            return NoContent();
        }

        // POST: api/Titles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Title>> PostTitle(Title title)
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

            title.CompanyId = companyId.Value;
            _context.Titles.Add(title);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TitleExists(title.Id))
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest(new { message = "Job Title is already present" });
                }
            }

            return CreatedAtAction("GetTitle", new { id = title.Id }, title);
        }

        // DELETE: api/Titles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Title>> DeleteTitle(int id)
        {
            var title = await _context.Titles.FindAsync(id);
            if (title == null)
            {
                return NotFound();
            }

            _context.Titles.Remove(title);
            await _context.SaveChangesAsync();

            return title;
        }

        private bool TitleExists(int id)
        {
            return _context.Titles.Any(e => e.Id == id);
        }
    }
}
