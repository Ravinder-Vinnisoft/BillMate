using BillMate.Data;
using BillMate.Models;
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
    public class DepartmentController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public DepartmentController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/Department
        [HttpGet]
        public async Task<ActionResult<List<Department>>> GetDepartments()
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

                var depts = await _context.Department.Include(d => d.JobTitles).ThenInclude(t => t.JobDuties).Where(x => x.CompanyId == companyId).OrderBy(d => d.Name).ToListAsync();
                return depts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: api/Department/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            var department = await _context.Department.FindAsync(id);

            if (department == null)
            {
                return NotFound();
            }

            return department;
        }

        // PUT: api/Department/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutDepartment([FromQuery] int id, Department department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }
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

                department.CompanyId = companyId;

                _context.Entry(department).State = EntityState.Modified;

                var storedTitles = _context.Titles.Include(t => t.JobDuties).AsNoTracking()
                    .Where(t => t.DepartmentId == department.Id).ToList();
                var removedTitles = storedTitles.Where(t1 => !department.JobTitles.Any(t2 => t2.Id == t1.Id));
                _context.Titles.RemoveRange(removedTitles);
                await _context.SaveChangesAsync();

                foreach (var title in department.JobTitles)
                {
                    title.CompanyId = companyId;
                    if (title.Id == default(int))
                    { _context.Entry(title).State = EntityState.Added; }
                    else
                    {
                        _context.Entry(title).State = EntityState.Modified;
                    }
                    var storedDuties = _context.Duty.AsNoTracking().Where(d => d.TitleId == title.Id).ToList();
                    var removedDuties = storedDuties.Where(d1 => !title.JobDuties.Any(d2 => d2.Id == d1.Id));
                    _context.Duty.RemoveRange(removedDuties);
                    await _context.SaveChangesAsync();


                    foreach (var duty in title.JobDuties)
                    {
                        duty.CompanyId = companyId;
                        if (duty.Id == default(int))
                        { _context.Entry(duty).State = EntityState.Added; }
                        else
                        { _context.Entry(duty).State = EntityState.Modified; }
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (DepartmentExists(department.Name))
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest(new { message = "Duplicate Department/Job Title for the department" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occured while updating the department. Try again." });
            }
            return NoContent();
        }

        // POST: api/Department
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult> PostDepartment(Department department)
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

                department.CompanyId = companyId.Value;
                _context.Department.Add(department);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException dbEx)
            {
                if (DepartmentExists(department.Name))
                {
                    return Conflict();
                }
                else
                {
                    return BadRequest(new { message = "Duplicate Department/Job Title for the department" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/Department/5
        [HttpDelete]
        public async Task<ActionResult<Department>> DeleteDepartment([FromQuery] int id)
        {
            try
            {
                var department = await _context.Department.Include(d => d.JobTitles).Where(d => d.Id == id).FirstOrDefaultAsync();
                if (department == null)
                {
                    return NotFound();
                }

                department.JobTitles.ToList().ForEach(t =>
                {
                    _context.Remove(t);
                });
                _context.Remove(department);

                await _context.SaveChangesAsync();

                return department;
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occured while deleting the department. Try again." });
            }
        }

        private bool DepartmentExists(string name)
        {
            return _context.Department.Any(d => d.Name == name);
        }
    }
}
