using BillMate.Data;
using BillMate.Helpers;
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
    public class EmployeesController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public EmployeesController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<string>>> GetManagerList()
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

            return await _context.Employee.Where(emp => emp.JobRole == "Manager" && emp.CompanyId == companyId).Select(e => new string(e.FirstName + (!string.IsNullOrEmpty(e.LastName) ? " " + e.LastName : ""))).ToListAsync();
        }

        // GET: api/Roles
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<string>>> GetTitleList()
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

            return await _context.Titles.Where(x => x.CompanyId == companyId).Select(e => new string(e.TitleName)).ToListAsync();
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee([FromQuery(Name = "role")] string role, [FromQuery(Name = "firstName")] string firstName)
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

                if (role == "Manager")
                {
                    Console.WriteLine("--------------Came in Manager-----------");
                    return await _context.Employee.Where(emp => emp.AssignedManager == firstName && emp.CompanyId == companyId)
                        .Include(a => a.AddressEmployee)
                        .Include(a => a.Department)
                        .ToListAsync();
                }

                return await _context.Employee.Include(a => a.User).Include(a => a.AddressEmployee).Include(a => a.Department).Where(x => x.CompanyId == companyId).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex });
            }
        }

        // GET: api/Employees/5
        [HttpGet("[action]")]
        public async Task<ActionResult<Employee>> GetEmployee([FromQuery] int id)
        {
            try
            {
                var employee = await _context.Employee
                        .Include(a => a.AddressEmployee).Include(a => a.User)
                        .AsNoTracking()
                        .FirstAsync(c => c.Id == id);

                if (employee == null)
                {
                    return NotFound();
                }
                var department = await _context.Department.Include(d => d.JobTitles).ThenInclude(t => t.JobDuties).Where(d => d.Id == employee.DepartmentId).FirstOrDefaultAsync();
                employee.Department = department;
                var jobTitle = department.JobTitles.Where(t => t.TitleName == employee.JobTitle).FirstOrDefault();

                var duties = jobTitle != null ? jobTitle.JobDuties.ToList() : new List<Duty>();
                employee.Duties = duties;
                return employee;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new Employee();
            }
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
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

            employee.CompanyId = companyId;
            employee.AddressEmployee.CompanyId = companyId;

            _context.Entry(employee).State = EntityState.Modified;
            _context.Entry(employee.AddressEmployee).State = EntityState.Modified;

            var employeeOffices = _context.EmployeeOffices.Where(x => x.EmployeeId == employee.Id).ToList();
            if (employeeOffices.Count > 0)
            {
                _context.EmployeeOffices.RemoveRange(employeeOffices);
                await _context.SaveChangesAsync();
            }

            foreach (var x in employee.AssignOffices.Split(','))
            {
                if (!string.IsNullOrEmpty(x))
                {
                    var emplOff = new EmployeeOffices();
                    emplOff.EmployeeId = employee.Id;
                    emplOff.ClientId = Convert.ToInt32(x);
                    _context.EmployeeOffices.Add(emplOff);
                    await _context.SaveChangesAsync();
                }
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

        // POST: api/Employees
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
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

                var isUserExist = _context.User.Where(x => x.Username == employee.WorkEmailAddress).FirstOrDefault();

                if (isUserExist != null)
                    return BadRequest(new { message = "Employee email address is already exist" });

                //user adding
                User user = new User();
                user.FirstName = employee.FirstName + (!string.IsNullOrEmpty(employee.LastName) ? " " + employee.LastName : "");
                user.Username = employee.WorkEmailAddress;
                user.Password = Encryption.CalculateMD5Hash("pass");
                user.Role = employee.JobRole;
                user.CompanyId = companyId.Value;
                _context.User.Add(user);
                await _context.SaveChangesAsync();

                employee.AddressEmployee.CompanyId = companyId.Value;
                _context.AddressEmployee.Add(employee.AddressEmployee);
                await _context.SaveChangesAsync();

                int addressId = _context.AddressEmployee.Where(a => a == employee.AddressEmployee)
                    .Select(a => a.Id).FirstOrDefault();
                employee.AddressId = addressId;
                employee.UserId = user.Id;
                employee.CompanyId = companyId.Value;
                employee.Department = null;
                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();

                foreach (var x in employee.AssignOffices.Split(','))
                {
                    if (!string.IsNullOrEmpty(x))
                    {
                        var emplOff = new EmployeeOffices();
                        emplOff.EmployeeId = employee.Id;
                        emplOff.ClientId = Convert.ToInt32(x);
                        _context.EmployeeOffices.Add(emplOff);
                        await _context.SaveChangesAsync();
                    }
                }



                return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(new { message = ex });
            }
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var address = await _context.AddressEmployee.FindAsync(employee.AddressId);
            if (address == null)
            {
                return NotFound();
            }

            var employeeOffices = _context.EmployeeOffices.Where(x => x.EmployeeId == employee.Id).ToList();
            if (employeeOffices.Count > 0)
            {
                _context.EmployeeOffices.RemoveRange(employeeOffices);
            }

            _context.AddressEmployee.Remove(address);
            _context.Employee.Remove(employee);

            if (employee.UserId.HasValue)
            {
                var user = await _context.User.FindAsync(employee.UserId);
                if (user != null)
                {
                    _context.User.Remove(user);
                }
            }

            await _context.SaveChangesAsync();

            return employee;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}