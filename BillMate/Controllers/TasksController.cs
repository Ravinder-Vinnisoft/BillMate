using BillMate.Data;
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
    public class TasksController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public TasksController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<BillMate.Models.Task>>> GetCompletedTask([FromQuery(Name = "role")] string role, [FromQuery(Name = "firstName")] string firstName, [FromQuery(Name = "username")] string username)
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

            return await _context.Tasks.Include(tsk => tsk.Client).Include(tsk => tsk.Employee).Where(tsk => tsk.TaskStatus == "completed" && tsk.CompanyId == companyId).ToListAsync();
        }

        // GET: api/Roles
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<BillMate.Models.Task>>> GetImpTask([FromQuery(Name = "role")] string role, [FromQuery(Name = "firstName")] string firstName, [FromQuery(Name = "username")] string username)
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

            return await _context.Tasks.Include(tsk => tsk.Client).Include(tsk => tsk.Employee).Where(tsk => tsk.TaskTag == "important" && tsk.CompanyId == companyId).ToListAsync();
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillMate.Models.Task>>> GetTasks([FromQuery(Name = "role")] string role, [FromQuery(Name = "firstName")] string firstName, [FromQuery(Name = "username")] string username)
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

            return await _context.Tasks.Include(tsk => tsk.Client).Include(tsk => tsk.Employee).Where(x => x.CompanyId == companyId).ToListAsync();
        }

        // GET: api/Tasks
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<BillMate.Models.Task>>> GetTaskByUniqueColumns([FromQuery(Name = "taskName")] string taskName, [FromQuery(Name = "taskType")] string taskType, [FromQuery(Name = "taskNature")] string taskNature)
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

            BillMate.Models.Task task = await _context.Tasks.Include(tsk => tsk.Client).Include(tsk => tsk.Employee).FirstOrDefaultAsync(tsk => tsk.CompanyId == companyId && tsk.TaskName == taskName && tsk.TaskType == taskType && tsk.TaskNature == taskNature);
            return Ok(task);
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BillMate.Models.Task>> GetTask(int id)
        {
            try
            {
                var task = await _context.Tasks.Where(t => t.Id == id).Include(t => t.Client).Include(t => t.Employee).FirstOrDefaultAsync();

                if (task == null)
                {
                    return NotFound();
                }

                return task;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, BillMate.Models.Task task)
        {
            if (id != task.Id)
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

            task.CompanyId = companyId;

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        // POST: api/Tasks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<BillMate.Models.Task>> PostTask(BillMate.Models.Task task)
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

            task.CompanyId = companyId.Value;

            _context.Tasks.Add(task);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TaskExists(task.Id))
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest(new { message = "Task is already present" });
                }
            }

            return CreatedAtAction("GetTask", new { id = task.Id }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BillMate.Models.Task>> DeleteTask(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return NotFound();
                }

                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();

                return task;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
