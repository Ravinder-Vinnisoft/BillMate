using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillMate.Data;
using BillMate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BillMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientDetailsController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public ClientDetailsController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/ClientDetails
        [HttpGet]
        public ActionResult<IEnumerable<ClientDetail>> GetClientDetails(int clientId)
        {
            try
            {
                List<ClientDetail> results = new List<ClientDetail>();
                var clientDetails = _context.ClientDetails.Where(x => x.ClientId == clientId).ToList();
                if(clientDetails.Any())
                {
                    var clientDetailTable = clientDetails.First();
                    var client = _context.Client.Find(clientDetailTable.ClientId);
                    ClientDetail clientDetail = new ClientDetail()
                    {
                        EmailName = client.Email,
                        LogoURL = client.PracticeLogo,
                        PracticeName = clientDetailTable.PracticeName,
                        AutomaticPostPayments = clientDetailTable.AutomaticPostPayments,
                        ClientId = clientId,
                        EndTime = clientDetailTable.EndTime,
                        StartTime = clientDetailTable.StartTime,
                        ExcludeBilling = clientDetailTable.ExcludeBilling,
                        Id = clientDetailTable.Id,
                        PracticePhoneNumber = client.PhoneNumber,
                        PracticeWebsite = clientDetailTable.PracticeWebsite,
                        ReplyTo = clientDetailTable.ReplyTo
                    };

                    results.Add(clientDetail);
                }
                return results;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        // POST: api/ClientDetails
        [HttpPost]
        public ActionResult<ClientDetail> AddClientDetails(ClientDetail details)
        {
            try
            {
                _context.ClientDetails.Add(details);
                _context.SaveChangesAsync();
                UpdateClientDetails(details.ClientId, details.PracticeName, details.LogoURL, details.PracticeAddress, details.PracticePhoneNumber);

                return details;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        private async System.Threading.Tasks.Task UpdateClientDetails(int clientId, string practiceName, string logoURL, string practiceAddress, string practicePhoneNumber)
        {
            var client = _context.Client.Find(clientId);
            if (client != null)
            {
                //client.PracticeName = practiceName;
                client.PracticeLogo = logoURL;
                client.PhoneNumber = practicePhoneNumber;
                _context.Entry(client).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
        // PUT: api/ClientDetails/2
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateClientDetails(int id, ClientDetail details)
        {
            try
            {
                if (id != details.Id)
                {
                    return BadRequest();
                }

                _context.Entry(details).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                    await UpdateClientDetails(details.ClientId, details.PracticeName, details.LogoURL, details.PracticeAddress, details.PracticePhoneNumber);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return BadRequest(new { message = ex.Message });
                }
                return NoContent();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
