using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenOnion.Server.DataLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenOnion.Server.DataLayer.DataMappers;
using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.Enums;
using GreenOnion.Server.DataLayer.DataAccess;

namespace Green_Onion.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        private readonly CompanyDataAccess _companyDataAccess;
        private readonly PredictionService predictionService;
        private readonly CompanyDataMapper _companyDataMapper;

        public CompanyController(CompanyDataAccess companyDataAccess, CompanyDataMapper companyDataMapper)
        {
            _companyDataAccess = companyDataAccess;
            this.predictionService = new PredictionService();
            _companyDataMapper = companyDataMapper;
        }


        // GET: api/Company
        [HttpGet]
        public IEnumerable<Company> GetCompanies()
        {
            return _companyDataAccess.SelectAll();
        }



        // GET: api/Company/getById/5
        [Route("getById/{id}")]
        [HttpGet]
        public ActionResult<Company> GetCompanyById(string id)
        {
            var company = _companyDataAccess.Select(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // Changes company data
        // PUT: api/Company/changeById/5
        [Route("changeById/{id}")]
        [HttpPut]
        public ActionResult<CompanyDto> PutCompany(string id, Company company)
        {
            if (id != company.companyId)
            {
                return BadRequest();
            }

            try
            {
                return _companyDataMapper.MapEntityToDto(_companyDataAccess.Update(id, company));
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        

        // POST: api/Company
        [HttpPost]
        public ActionResult<CompanyDto> PostCompany(Company company)
        {
            _context.companies.Add(company);
            User user = _context.users.Find(company.userId);

            user.companyId = company.companyId;

            company.Employees.Add(user);

            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CompanyExists(company.companyId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction(nameof(GetCompany), new { id = company.companyId }, company);
        }

        private bool CompanyExists(string id)
        {
            return _context.companies.Any(e => e.companyId == id);
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
            }
            else
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
                member.Tickets.ForEach(delegate (Ticket ticket)
                {
                    if (ticket.projectId == project.projectId)
                    {
                        _ = member.Tickets.Remove(ticket);
                    }
                });
            });
        }

        private void RemoveTicketFromProject(Project project)
        {
            project.Tickets.ForEach(delegate (Ticket ticket)
            {
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

            await _context.companies.ForEachAsync(delegate (Company company)
            {
                if (company.Employees.Contains(employee))
                {
                    companies.Add(company);
                }
            });

            return companies;
        }
    }
}
