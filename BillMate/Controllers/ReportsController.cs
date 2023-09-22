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
    public class ReportsController : ControllerBase
    {
        private readonly BillMateDBContext _context;

        public ReportsController(BillMateDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetDentalInsuranceSummedByCarrier")]
        public async Task<ActionResult<IEnumerable<DentalInsuranceSummedByCarrier>>> GetEmployeeDentalInsuranceSummedByCarrier()
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

                var lst = await _context.DentalInsuranceSummedByCarrier.Include(x => x.Client).Where(x => x.CompanyId == companyId).ToListAsync();

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<DentalInsuranceSummedByCarrier>();
            }
        }

        [HttpGet]
        [Route("GetDentalOutstandingClaims")]
        public async Task<ActionResult<IEnumerable<DentalOutstandingClaims>>> GetDentalOutstandingClaims()
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

                var lst = await _context.DentalOutstandingClaims.Include(x => x.Client).Where(x => x.CompanyId == companyId).ToListAsync();

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<DentalOutstandingClaims>();
            }
        }


        [HttpGet]
        [Route("GetDentalOutstandingPreAuth")]
        public async Task<ActionResult<IEnumerable<DentalOutstandingPreAuth>>> GetDentalOutstandingPreAuth()
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

                var lst = await _context.DentalOutstandingPreAuth.Include(x => x.Client).Where(x => x.CompanyId == companyId).ToListAsync();

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<DentalOutstandingPreAuth>();
            }
        }


        [HttpGet]
        [Route("GetDentalTotalAdjustments")]
        public async Task<ActionResult<IEnumerable<DentalTotalAdjustments>>> GetDentalTotalAdjustments()
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

                var lst = await _context.DentalTotalAdjustments.Include(x => x.Client).Where(x => x.CompanyId == companyId).ToListAsync();

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<DentalTotalAdjustments>();
            }
        }

        [HttpGet]
        [Route("GetDentalTotalClaims")]
        public async Task<ActionResult<IEnumerable<DentalTotalClaims>>> GetDentalTotalClaims()
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

                var lst = await _context.DentalTotalClaims.Include(x => x.Client).Where(x => x.CompanyId == companyId).ToListAsync();

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<DentalTotalClaims>();
            }
        }

        [HttpGet]
        [Route("GetDentalWriteOffs")]
        public async Task<ActionResult<IEnumerable<DentalWriteOffs>>> GetDentalWriteOffs()
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

                var lst = await _context.DentalWriteOffs.Include(x => x.Client).Where(x => x.CompanyId == companyId).ToListAsync();

                return lst;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<DentalWriteOffs>();
            }
        }
    }
}