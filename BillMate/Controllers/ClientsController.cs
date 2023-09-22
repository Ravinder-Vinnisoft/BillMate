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
    public class ClientsController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public ClientsController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClient([FromQuery(Name = "role")] string role, [FromQuery(Name = "firstName")] string firstName, [FromQuery(Name = "username")] string username)
        {
            try
            {
                var user = HttpContext.User;
                string companyIdString = null;
                int? companyId = null;
                if(user != null)
                {
                    if(user.HasClaim(x => x.Type == "CompanyId"))
                    {
                        companyIdString = user.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value;
                        companyId = Convert.ToInt32(companyIdString);
                    }
                } else
                {
                    throw new Exception("User is not linked to a company");
                }

                if (role == "Manager")
                {
                    Console.WriteLine("--------------Came in Client Manager-----------");
                    List<string> clients = await _context.Employee.Where(emp => emp.AssignedManager == firstName).Select(e => new string((!string.IsNullOrEmpty(e.AssignOffices) ? e.AssignOffices : ""))).ToListAsync();
                    Console.WriteLine("--------------clients list-----------" + string.Join(",", clients));
                    HashSet<int> set = new HashSet<int>();
                    foreach (string cli in clients)
                    {
                        string[] strings = cli.Split(",");
                        foreach (string s in strings)
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                set.Add(Convert.ToInt32(s));
                            }
                        }
                    }
                    Console.WriteLine("--------------clients set-----------" + string.Join(",", set));
                    return await _context.Client.Where(x => set.Contains(x.Id) && x.CompanyId == companyId).Include(a => a.User).Include(a => a.AddressDetails).Include(b => b.BillingPreferences).Include(n => n.NotificationsPreferences).ToListAsync();

                }
                else if (role == "Employee")
                {
                    Console.WriteLine("--------------Came in Client Employee-----------");
                    List<string> clients = await _context.Employee.Where(emp => emp.WorkEmailAddress == username).Select(e => new string((!string.IsNullOrEmpty(e.AssignOffices) ? e.AssignOffices : ""))).ToListAsync();
                    Console.WriteLine("--------------clients list-----------" + string.Join(",", clients));
                    HashSet<int> set = new HashSet<int>();
                    foreach (string cli in clients)
                    {
                        string[] strings = cli.Split(",");
                        foreach (string s in strings)
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                set.Add(Convert.ToInt32(s));
                            }
                        }
                    }
                    Console.WriteLine("--------------clients set-----------" + string.Join(",", set));
                    return await _context.Client.Where(x => set.Contains(x.Id) && x.CompanyId == companyId).Include(a => a.User).Include(a => a.AddressDetails).Include(b => b.BillingPreferences).Include(n => n.NotificationsPreferences).ToListAsync();
                }

                var clientsData = await _context.Client
                                .Where(x => x.CompanyId == companyId)
                                .Include(a => a.AddressDetails)
                                .Include(a => a.User)
                                .Include(b => b.BillingPreferences)
                                .Include(n => n.NotificationsPreferences)
                                .ToListAsync();

                foreach (var item in clientsData)
                {
                    var employeeOffice = _context.EmployeeOffices.FirstOrDefault(x => x.ClientId == item.Id);
                    if (employeeOffice == null)
                        continue;

                    var employee = _context.Employee.FirstOrDefault(x => x.Id == employeeOffice.EmployeeId);
                    item.Employee = employee;
                }

                return clientsData;
            } catch(Exception exc)
            {
                throw exc;
            }
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
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

                var client = await _context.Client.Include(a => a.AddressDetails).Include(a => a.User).Include(b => b.BillingPreferences).Include(n => n.NotificationsPreferences).AsNoTracking().FirstAsync(c => c.Id == id && c.CompanyId == companyId);

                if (client == null)
                {
                    return NotFound();
                }
                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return BadRequest(new { message = ex.InnerException.Message });
            }
        }

        // GET: api/Clients/5
        [HttpGet("byUserId/{userId}")]
        public async Task<ActionResult<Client>> GetClientDetailsByUserId(int userId)
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

                var client = await _context
                                    .Client
                                    .Include(a => a.AddressDetails)
                                    .Include(a => a.User)
                                    .Include(b => b.BillingPreferences)
                                    .Include(n => n.NotificationsPreferences)
                                    .AsNoTracking()
                                    .FirstAsync(c => c.UserId == userId && c.CompanyId == companyId);

                if (client == null)
                {
                    return NotFound();
                }
                return client;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
                return BadRequest(new { message = ex.InnerException.Message });
            }
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Id)
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

            client.CompanyId = companyId;
            client.AddressDetails.CompanyId = companyId;
            client.BillingPreferences.CompanyId = companyId;
            client.NotificationsPreferences.CompanyId = companyId;

            _context.Entry(client).State = EntityState.Modified;
            _context.Entry(client.AddressDetails).State = EntityState.Modified;
            _context.Entry(client.BillingPreferences).State = EntityState.Modified;
            _context.Entry(client.NotificationsPreferences).State = EntityState.Modified;

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

        // POST: api/Clients
        // To protect from oveposting attacks, please enable the specific properties to bind to, for
        // more details if you want see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            try
            {
                var currentUser = HttpContext.User;
                string companyIdString = null;
                int? companyId = null;
                if (currentUser != null)
                {
                    if (currentUser.HasClaim(x => x.Type == "CompanyId"))
                    {
                        companyIdString = currentUser.Claims.FirstOrDefault(x => x.Type == "CompanyId")?.Value;
                        companyId = Convert.ToInt32(companyIdString);
                    }
                }
                else
                {
                    throw new Exception("User is not linked to a company");
                }

                var isUserExist = _context.User.Where(x => x.Username == client.Email).FirstOrDefault();

                if (isUserExist != null)
                    return BadRequest(new { message = "Client email address is already exist" });


                //user adding
                User user = new User();
                user.FirstName = client.PracticeName;
                user.Username = client.Email;
                user.Password = Encryption.CalculateMD5Hash("pass");
                user.Role = "Client";
                user.CompanyId = companyId.Value;
                _context.User.Add(user);
                await _context.SaveChangesAsync();

                client.UserId = user.Id;
                client.CompanyId = companyId.Value;
                _context.Client.Add(client);
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

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var address = await _context.AddressDetails.FindAsync(client.AddressId);
            if (address == null)
            {
                return NotFound();
            }

            var billing = await _context.BillingPreferences.FindAsync(client.BillingId);
            if (billing == null)
            {
                return NotFound();
            }

            var notification = await _context.NotificationsPreferences.FindAsync(client.NotificationId);
            if (notification == null)
            {
                return NotFound();
            }


            _context.Client.Remove(client);
            _context.AddressDetails.Remove(address);
            _context.BillingPreferences.Remove(billing);
            _context.NotificationsPreferences.Remove(notification);
            if (client.UserId.HasValue)
            {
                var user = await _context.User.FindAsync(client.UserId);
                if (user != null)
                {
                    _context.User.Remove(user);
                }
            }

            await _context.SaveChangesAsync();

            return client;
        }

        private bool ClientExists(int id)
        {
            return _context.Client.Any(e => e.Id == id);
        }
    }
}