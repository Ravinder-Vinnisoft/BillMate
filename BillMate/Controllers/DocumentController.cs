using BillMate.Data;
using BillMate.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BillMate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly BillMateDBContext _context;
        private readonly IWebHostEnvironment _hostingEnv;

        public DocumentController(IWebHostEnvironment hostingEnv, BillMateDBContext context)
        {
            _context = context;
            _hostingEnv = hostingEnv;
        }

        [HttpGet()]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocumentsLibrary([FromQuery] int sortOrder, [FromQuery] string tagFilter, [FromQuery] string docFilter, int userId)
        {
            List<Document> docs = new List<Document>();
            if (tagFilter == "null") { tagFilter = null; }
            if (docFilter == "null") { docFilter = null; }

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

            var user = _context.User.FirstOrDefault(x => x.Id == userId && x.CompanyId == companyId);
            IQueryable<Document> query = _context.Document.Where(x => x.CompanyId == companyId);

            switch (user.Role)
            {
                case "Admin":

                    query = query.Select(d => new Document
                    {
                        Id = d.Id,
                        FileName = d.FileName,
                        FileType = d.FileType,
                        Tag = d.Tag,
                        StoredTime = d.StoredTime,
                        ClientId = d.ClientId
                    }).Where(x => x.ClientId != 0);
                    break;

                case "Manager":

                    var clientIds = _context.EmployeeOffices
                                            .Include(x => x.Employee)
                                            .Where(x => x.EmployeeId == x.Employee.Id && x.Employee.JobRole == "Employee"
                                             && x.Employee.AssignedManager == user.FirstName)
                                            .Select(x => x.ClientId)
                                            .ToList();

                    query = query.Select(d => new Document
                    {
                        Id = d.Id,
                        FileName = d.FileName,
                        FileType = d.FileType,
                        Tag = d.Tag,
                        StoredTime = d.StoredTime,
                        ClientId = d.ClientId
                    }).Where(x => clientIds.Contains(x.ClientId));

                    break;

                case "Employee":

                    var clientIdsForEmployee = _context.EmployeeOffices
                                            .Include(x => x.Employee)
                                            .Where(x => x.EmployeeId == x.Employee.Id && x.Employee.JobRole == "Employee"
                                             && x.Employee.UserId == userId)
                                            .Select(x => x.ClientId)
                                            .ToList();

                    query = query.Select(d => new Document
                    {
                        Id = d.Id,
                        FileName = d.FileName,
                        FileType = d.FileType,
                        Tag = d.Tag,
                        StoredTime = d.StoredTime,
                        ClientId = d.ClientId
                    }).Where(x => clientIdsForEmployee.Contains(x.ClientId));

                    break;

                case "Client":

                    int? clientId = _context.Client.Where(c => c.UserId == userId).Select(x => x.Id).FirstOrDefault();

                    query = query.Select(d => new Document
                    {
                        Id = d.Id,
                        FileName = d.FileName,
                        FileType = d.FileType,
                        Tag = d.Tag,
                        StoredTime = d.StoredTime,
                        ClientId = d.ClientId
                    }).Where(x => x.ClientId == clientId);

                    break;
                default:
                    break;
            }
            

            // if tags filters are provided
            if (!string.IsNullOrEmpty(tagFilter))
            {
                query = query.Where(d => d.Tag == tagFilter);
            }

            // if docs filter is provided
            if (!string.IsNullOrEmpty(docFilter))
            {
                query = query.Where(d => d.FileName.Contains(docFilter));
            }

            docs = await query.ToListAsync();

            return sortOrder == 0 ? docs.OrderByDescending(d => d.StoredTime).ToList() : docs.OrderBy(d => d.StoredTime).ToList();
        }

        /// <summary>
        /// API to get documents by client
        /// </summary>
        /// <param name="sortOrder">Ordering the documents</param>
        /// <param name="clientId">Id of the client to fetch docs</param>
        /// <param name="tagFilter">tags to filter docs</param>
        /// <param name="docFilter">document name to filter docs</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("[action]")]
        public ActionResult<IEnumerable<Document>> GetDocumentsClientLibrary([FromQuery] int sortOrder, int? userId, [FromQuery] string tagFilter, [FromQuery] string docFilter)
        {
            List<Document> docs = new List<Document>();
            if (tagFilter == "null") { tagFilter = null; }
            if (docFilter == "null") { docFilter = null; }

            if (!userId.HasValue)
                return BadRequest();

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

            var user = _context.User.FirstOrDefault(x => x.Id == userId && x.CompanyId == companyId);
            var clients = _context.Client.Where(x => x.CompanyId == companyId).ToList();
            IEnumerable<Document> query = _context.Document.Where(x => x.CompanyId == companyId);

            switch (user.Role)
            {
                case "Admin":

                    docs = _context.Document.FromSqlRaw($@"SELECT * FROM Document WHERE UploadedByUserId IN (
                                                            SELECT UserId FROM Client WHERE CompanyId = '{companyId}')").ToList();

                    break;

                case "Manager":

                    docs = _context.Document.FromSqlRaw($@"SELECT * FROM Document WHERE UploadedByUserId IN (
                                                            SELECT UserId FROM Client WHERE Id IN(
                                                            SELECT
                                                                ClientId
                                                            FROM
                                                                EmployeeOffices eo
                                                                LEFT JOIN Employee e ON e.Id = eo.EmployeeId
                                                            WHERE
                                                                e.JobRole = 'Employee'
                                                                AND e.AssignedManager = '{user.FirstName}'
                                                                AND e.CompanyId = '{companyId}'
                                                        ))").ToList();

                    break;

                case "Employee":

                    docs = _context.Document.FromSqlRaw($@"SELECT * FROM Document WHERE UploadedByUserId IN (
                                                            SELECT UserId FROM Client WHERE Id IN(
                                                            SELECT
                                                                ClientId
                                                            FROM
                                                                EmployeeOffices eo
                                                                LEFT JOIN Employee e ON e.Id = eo.EmployeeId
                                                            WHERE
                                                                e.JobRole = 'Employee'
                                                                AND e.UserId = '{userId}'
                                                                AND e.CompanyId = '{companyId}'
                                                        ))").ToList();

                    break;

                case "Client":

                    docs = _context.Document.FromSqlRaw($@"SELECT * FROM Document WHERE UploadedByUserId = '{userId}'").ToList();

                    break;
                default:
                    break;
            }

            // if sort order is provided then sort documents accordingly
            return sortOrder == 0 ? docs.OrderByDescending(d => d.StoredTime).ToList() : docs.OrderBy(d => d.StoredTime).ToList();
        }

        [HttpGet("{id}")]
        [Route("[action]")]
        public async Task<ActionResult<Document>> GetDocument([FromQuery] int id)
        {
            return await _context.Document.FindAsync(id);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<string>>> GetTags([FromQuery] string tagFilter)
        {
            if (tagFilter == "null")
            {
                tagFilter = null;
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

            if (!string.IsNullOrEmpty(tagFilter))
            {
                return await _context.Document.Select(d => d.Tag).Where(t => t != null && t.Contains(tagFilter)).Distinct().ToListAsync();
            }
            else
            {
                return await _context.Document.Select(d => d.Tag).Where(t => t != null).Distinct().ToListAsync();
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<string>>> GetAssignedTags([FromQuery] string fileName)
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

            return await _context.Document.Where(d => d.FileName == fileName && d.CompanyId == companyId && d.Tag != null).Select(d => d.Tag).Distinct().ToListAsync();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<IEnumerable<Client>>> GetAssignedClients([FromQuery] string fileName)
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

            var clientIds = await _context.Document.Where(d => d.FileName == fileName && d.CompanyId == companyId && d.ClientId != 0).Select(d => d.ClientId).Distinct().ToListAsync();
            return await _context.Client.Where(c => clientIds.Contains(c.Id)).ToListAsync();
        }


        [HttpPost()]
        [Route("[action]")]
        public async Task<ActionResult<int>> UpdateDocumentMeta([FromQuery] string fileName, [FromQuery] string tags, [FromQuery] string clients)
        {
            try
            {
                var selectedTags = !string.IsNullOrEmpty(tags) ? tags.Split(",").ToList() : new List<string>();
                var selectedClients = !string.IsNullOrEmpty(clients) ? clients.Split(",").Distinct().ToList() : new List<string>();

                var storedMeta = await _context.Document.Where(d => d.FileName == fileName).Select(d => new Document
                {
                    Id = d.Id,
                    FileName = d.FileName,
                    FileType = d.FileType,
                    Tag = d.Tag,
                    ClientId = d.ClientId
                }).ToListAsync();

                // Associated file's data
                var file = _context.Document.Where(d => d.Id == storedMeta[0].Id).Select(d => d.File).FirstOrDefault();
                var fileMeta = storedMeta[0];

                // Remove exisiting rows for the file
                storedMeta.ForEach(d =>
                {
                    _context.Document.Remove(d);
                });
                await _context.SaveChangesAsync();

                if (selectedTags.Count() > 0)
                {
                    // Add new rows for the incoming tags and clients
                    selectedTags.ForEach(t =>
                        {
                            if (selectedClients.Count() > 0)
                            {
                                selectedClients.ForEach(c =>
                                {
                                    Document doc = new Document
                                    {
                                        FileName = fileMeta.FileName,
                                        File = file,
                                        FileType = fileMeta.FileType,
                                        ClientId = int.Parse(c),
                                        Tag = t
                                    };
                                    _context.Document.Add(doc);
                                });
                            }
                            else
                            {
                                Document doc = new Document
                                {
                                    FileName = fileMeta.FileName,
                                    File = file,
                                    FileType = fileMeta.FileType,
                                    ClientId = 0,
                                    Tag = t
                                };
                                _context.Document.Add(doc);
                            }
                        });
                }
                else
                {
                    if (selectedClients.Count() > 0)
                    {
                        selectedClients.ForEach(c =>
                        {
                            Document doc = new Document
                            {
                                FileName = fileMeta.FileName,
                                File = file,
                                FileType = fileMeta.FileType,
                                ClientId = int.Parse(c),
                                Tag = null
                            };
                            _context.Document.Add(doc);
                        });
                    }
                    else
                    {
                        Document doc = new Document
                        {
                            FileName = fileMeta.FileName,
                            File = file,
                            FileType = fileMeta.FileType,
                            ClientId = 0,
                            Tag = null
                        };
                        _context.Document.Add(doc);
                    }

                }
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        //TODO: Change return type
        public async Task<ActionResult<Document>> PostDocuments([FromQuery(Name = "tags")] string tags, [FromQuery(Name = "clientIds")] string clientIds, int userId)
        {
            string result = "";
            try
            {
                var httpRequest = HttpContext.Request;
                var documents = httpRequest.Form.Files;

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

                if (documents.Count > 0)
                {
                    foreach (var doc in documents)
                    {
                        var fileName = Path.GetFileName(doc.FileName);

                        var assignedTo = !string.IsNullOrEmpty(clientIds) ? clientIds.Split(",").ToList() : new List<string>();
                        var selectedTags = !string.IsNullOrEmpty(tags) ? tags.Split(",").ToList() : new List<string>();

                        var storedMeta = await _context.Document.Where(d => d.FileName == fileName).Select(d => new Document
                        {
                            Id = d.Id,
                            FileName = d.FileName,
                            FileType = d.FileType,
                            Tag = d.Tag,
                            ClientId = d.ClientId,
                            UploadedByUserId = d.UploadedByUserId
                        }).ToListAsync();

                        // Remove exisiting rows for the file
                        storedMeta.ForEach(d =>
                        {
                            _context.Document.Remove(d);
                        });
                        await _context.SaveChangesAsync();

                        if (selectedTags.Count() > 0)
                        {
                            foreach (var tag in selectedTags)
                            {
                                await SaveDocumentPerClient(assignedTo, doc, fileName, tag, userId, companyId.Value);
                            }
                        }
                        else
                        {
                            await SaveDocumentPerClient(assignedTo, doc, fileName, null, userId, companyId.Value);
                        }

                    }
                    int i = _context.SaveChanges();
                    if (i > 0)
                    {
                        result = "Uploaded sucessfully";
                    }
                    else
                    {
                        result = "Upload failed";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
            return Ok(result);
        }

        private async System.Threading.Tasks.Task SaveDocumentPerClient(List<string> assignedTo, IFormFile doc, string fileName, string tag, int userId, int companyId)
        {
            if (assignedTo.Count() > 0)
            {
                foreach (var client in assignedTo)
                {
                    await SaveDocument(doc, fileName, tag, int.Parse(client), userId, companyId);
                }
            }
            else
            {
                await SaveDocument(doc, fileName, tag, 0, userId, companyId);
            }
        }

        private async System.Threading.Tasks.Task SaveDocument(IFormFile doc, string fileName, string tag, int clientId, int userId, int companyId)
        {
            Document document = new Document
            {
                FileName = fileName,
                FileType = Path.GetExtension(fileName),
                Tag = tag == "null" ? null : tag,
                ClientId = clientId,
                UploadedByUserId = userId,
                CompanyId = companyId
            };
            using (var target = new MemoryStream())
            {
                await doc.CopyToAsync(target);
                document.File = target.ToArray();
            }
            if (DocumentExists(document))
            {
                _context.Document.Update(document);
            }
            else
            {
                _context.Document.Add(document);
            }
        }

        [HttpDelete("{id}")]
        [Route("[action]")]
        public async Task<ActionResult<Document>> DeleteDocument([FromQuery] int id)
        {
            var document = await _context.Document.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            var documents = await _context.Document.Where(d => d.FileName == document.FileName).ToListAsync();
            documents.ForEach(d => _context.Document.Remove(d));
            await _context.SaveChangesAsync();

            return document;
        }

        private bool DocumentExists(Document document)
        {
            return _context.Document.Any(e => e.FileName == document.FileName && e.Tag == document.Tag);
        }
    }
}
