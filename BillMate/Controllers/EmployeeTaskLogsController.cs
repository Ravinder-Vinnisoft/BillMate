using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BillMate.Data;
using BillMate.Models;
using PostmarkDotNet;
using PostmarkDotNet.Model;
using System.Collections.Specialized;

namespace BillMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeTaskLogsController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public EmployeeTaskLogsController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeTaskLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeTaskLogs>>> GetEmployeeTaskLogs()
        {
            return await _context.EmployeeTaskLogs.ToListAsync();
        }

        // GET: api/EmployeeTaskLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeTaskLogs>> GetEmployeeTaskLogs(int id)
        {
            var employeeTaskLogs = await _context.EmployeeTaskLogs.FindAsync(id);

            if (employeeTaskLogs == null)
            {
                return NotFound();
            }

            return employeeTaskLogs;
        }

        // PUT: api/EmployeeTaskLogs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeTaskLogs(int id, EmployeeTaskLogs employeeTaskLogs)
        {
            if (id != employeeTaskLogs.Id)
            {
                return BadRequest();
            }

            _context.Entry(employeeTaskLogs).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeTaskLogsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/EmployeeTaskLogs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<EmployeeTaskLogs>> PostEmployeeTaskLogs(EmployeeTaskLogs employeeTaskLogs)
        {
            // Send an email asynchronously:
            Console.WriteLine("Inside task logs service");
            var message2 = new PostmarkMessage()
            {
                To = "snegi2416@conestogac.on.ca",
                From = "snegi2416@conestogac.on.ca",
                TrackOpens = true,
                Subject = employeeTaskLogs.TaskName,
                TextBody = employeeTaskLogs.Message,
                HtmlBody = "HTML goes here",
                Tag = "Help Center Email Campaign",
                //Headers = new HeaderCollection {  "CUSTOM-HEADER:value"  }
            };

            var client = new PostmarkClient("f83d8955-2f6d-48da-9001-d5b749377ec7");
            var sendResult = await client.SendMessageAsync(message2);

            if (sendResult.Status == PostmarkStatus.Success) { Console.WriteLine("message success");/* Handle success */ }
            else { /* Resolve issue.*/ }
            _context.EmployeeTaskLogs.Add(employeeTaskLogs);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeTaskLogs", new { id = employeeTaskLogs.Id }, employeeTaskLogs);
        }

        // DELETE: api/EmployeeTaskLogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeTaskLogs>> DeleteEmployeeTaskLogs(int id)
        {
            var employeeTaskLogs = await _context.EmployeeTaskLogs.FindAsync(id);
            if (employeeTaskLogs == null)
            {
                return NotFound();
            }

            _context.EmployeeTaskLogs.Remove(employeeTaskLogs);
            await _context.SaveChangesAsync();

            return employeeTaskLogs;
        }

        private bool EmployeeTaskLogsExists(int id)
        {
            return _context.EmployeeTaskLogs.Any(e => e.Id == id);
        }
    }
}
