using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenOnion.Server.DataLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenOnion.Server;
using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.Enums;

namespace Green_Onion.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {        
        public CompanyController(GreenOnionContext context)
        {
            _context = context;
            this.predictionService = new PredictionService();
        }

        private readonly GreenOnionContext _context;
        private readonly PredictionService predictionService;

        // GET: api/Company
        [Route("all")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> Getcompanies()
        {
            return await _context.companies.ToListAsync();
        }

        

        // GET: api/Company/5
        [Route("getById/{id}")]
        [HttpGet]
        public async Task<ActionResult<Company>> GetCompany(string id)
        {
            var company = await _context.companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/Company/5
        [Route("changeCompanyById/{id}")]
        [HttpPut]
        public async Task<IActionResult> PutCompany(string id, Company company)
        {
            if (id != company.CompanyId)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // GET: api/Prediction
        [Route("makepPrediction/projectId/{projectId}/companyId/{companyId}")]
        [HttpGet]
        public async Task<ActionResult<PredictionDto>> MakePrediction(string projectId, string companyId)
        {
            PredictionDto prediction = new PredictionDto();

            Company company = await _context.companies.FindAsync(companyId);

            // project is used to make prediction for it by its tickets complexy
            Project project = await _context.projects.FindAsync(projectId);
            prediction.DurationByTicketComplexity = predictionService.CalculateDurationByTicketComplexity(projectId, _context).ToString();

            prediction.NumOfMembers = predictionService.CalculateRequiredNum(PredictionType.Members.ToString(), company.Projects);
            prediction.NumOfTickets = predictionService.CalculateRequiredNum(PredictionType.Tickets.ToString(), company.Projects);

            prediction.DurationByHistoricalData = predictionService.CalculateDurationByHistoricalData(projectId, companyId, _context).ToString();

            return prediction;
        }

        // POST: api/Company
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            _context.companies.Add(company);
            User user = _context.users.Find(company.UserId);

            user.companyId = company.CompanyId;

            company.Employees.Add(user);

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CompanyExists(company.CompanyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetCompany), new { id = company.CompanyId }, company);
        }

        private bool CompanyExists(string id)
        {
            return _context.companies.Any(e => e.CompanyId == id);
        }

        // Add employee into the company
        // PUT: api/Company
        [Route("addEmployee/{userId}/companyId/{companyId}")]
        [HttpPut]
        public async Task<ActionResult<Company>> AddEmployee(string companyID, string userId)
        {
            Company company = _context.companies.Find(companyID);
            User user = _context.users.Find(userId);

            if (company is not null && (user is not null))
            {
                company.Employees.Add(user);

                _context.Entry(company).State = EntityState.Modified;

                _context.SaveChanges();

                return company;
            } else
            {
                return null;
            }
            
        }

        // Add project into the company
        // PUT: api/Company
        [Route("addProject/projectId/inCompany/{companyId}")]
        [HttpPut]
        public async Task<ActionResult<Company>> AddProject(string companyId, string projectId)
        {
            Company company = await _context.companies.FindAsync(companyId);
            Project project = await _context.projects.FindAsync(projectId);

            if (company is not null && project is not null)
            {
                company.Projects.Add(project);

                _context.Entry(company).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return company;
            }
            else
            {
                return null;
            }
        }

        // Removes project from company. Deletes project from DB. Call this api to delete project.
        // PUT: api/Company
        [Route("removeProject/{projectId}/fromCompany/{companyId}")]
        [HttpPut]
        public ActionResult<Company> RemoveProject(string copmanyId, string projectId)
        {
            Company company = _context.companies.Find(copmanyId);
            Project project = company.Projects.Find(proj => proj.ProjectId == projectId);

            RemoveProjectTicketsFromAssignees(project);
            RemoveTicketFromProject(project);

            company.Projects.Remove(project);

            _context.projects.Remove(project);
            _context.SaveChanges();

            return company;
        }

        private void RemoveProjectTicketsFromAssignees(Project project)
        {
            project.Members.ForEach(delegate (User member)
            {
                member.Tickets.ForEach(delegate (Ticket ticket) {
                    if (ticket.ProjectId == project.ProjectId)
                    {
                        _ = member.Tickets.Remove(ticket);
                    }
                });
            });
        }

        private void RemoveTicketFromProject(Project project)
        {
            project.Tickets.ForEach(delegate (Ticket ticket) {
                _context.tickets.Remove(ticket);
            });
        }



        // Get companies by userId
        // GET: api/Company
        [Route("getCompanyByUserId/{userId}")]
        [HttpGet]
        public async Task<ActionResult<List<Company>>> GetCompanyByUserId(string userId)
        {
            List<Company> companies = new List<Company>();
            User employee = await _context.users.FindAsync(userId);

            await _context.companies.ForEachAsync(delegate (Company company) {
                if (company.Employees.Contains(employee))
                {
                    companies.Add(company);
                }
            });

            return companies;
        }
    }
}
