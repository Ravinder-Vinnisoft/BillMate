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
    public class SchedulerConfigurationController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public SchedulerConfigurationController(BillMateDBContext context)
        {
            _context = context;
        }

        // GET: api/SchedulerConfiguration
        [HttpGet]
        public ActionResult<IEnumerable<SchedulerConfiguration>> GetSchedulerConfiguration(int clientId)
        {
            try
            {
                var configs = _context.SchedulerConfiguration.Where(x => x.ClientId == clientId).OrderBy(x => x.Id).ToList();

                return configs;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }


        // POST: api/SchedulerConfiguration
        [HttpPost]
        public ActionResult<SchedulerConfiguration> AddSchedulerConfiguration(SchedulerConfiguration config)
        {
            try
            {
                config.StoredTime = DateTime.UtcNow;
                _context.SchedulerConfiguration.Add(config);
                _context.SaveChangesAsync();
                return config;
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        // PUT: api/SchedulerConfiguration/2
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateSchedulerConfiguration(int id, SchedulerConfiguration config)
        {
            try
            {
                if (id != config.Id)
                {
                    return BadRequest();
                }

                _context.Entry(config).State = EntityState.Modified;
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
            catch (Exception exc)
            {
                throw exc;
            }
        }
    }
}
