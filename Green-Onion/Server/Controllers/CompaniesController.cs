using GreenOnion.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GreenOnion.Server.DataLayer.DataAccess;
using System.Linq;
using System.Collections.Generic;
using GreenOnion.Server.Datalayer.Dataaccess;

namespace GreenOnion.Server.Controllers
{
    [Route("api/CompaniesController")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {

        private readonly CompanyDbContext _companyContext;
        private readonly ProjectDbContext _projectContext;
        private readonly UserDbContext _userContext;
        private readonly TicketDbContext _ticketContext;

        public CompaniesController(CompanyDbContext context, UserDbContext userContext, ProjectDbContext projectContext, TicketDbContext ticketContext)
        {
            this._companyContext = context;
            this._userContext = userContext;
            this._ticketContext = ticketContext;
            this._projectContext = projectContext;
        }

        // Create company
        // POST: api/Company
        [HttpPost]
        [Route("{creatorId}/{projectId}")]
        public async Task<ActionResult<Company>> CreateCompany(string creatorId, string projectId, Company company)
        {

            _companyContext.companies.Add(company);
            await _companyContext.SaveChangesAsync();

            // user who creates company in app
            User companyCreator = await _userContext.users.FindAsync(creatorId);
            companyCreator.Companies.Add(company);
            await _userContext.SaveChangesAsync();

            // add project's company
            Project project = await _projectContext.projects.FindAsync(projectId);
            project.CompanyID = company.CompanyID;
            await _projectContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompanyById), new { id = company.CompanyID }, company);
        }

        // Add project into the context
        // PUT: api/Company
        [HttpPut("{companyId}")]
        public async Task<ActionResult<Company>> AddProject(string companyID, Project project)
        {
            Company company = await _companyContext.companies.FindAsync(companyID);
            company.Projects.Add(project);

            // add project to DB
            _projectContext.projects.Add(project);
            await _projectContext.SaveChangesAsync();

            try
            {
                await _companyContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(companyID))
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

        // Get projects of company by its id
        // GET: api/Company
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Project>>> GetProjects(string companyID)
        {
            Company company = await _companyContext.companies.FindAsync(companyID);

            return company.Projects;
        }

        // Delete company by id
        // DELETE: api/Company
        [HttpDelete("{companyId}")]
        public async Task<ActionResult> DeleteCompany(string companyID)
        {

            Company company = await _companyContext.companies.FindAsync(companyID);

            company.Projects.ForEach(delegate (Project project) {

                project.Tickets.ForEach(delegate (Ticket ticket) {

                    // remove ticket reference from assignees
                    User ticketAssignee = _userContext.users.Find(ticket.AssigneeID);
                    ticketAssignee.AssignedTickets.Remove(ticket);

                    // remove project reference from assignees
                    ticketAssignee.AssignedProjects.Remove(project);
                    ticket.ProjectID = null;

                    // remove member references from project
                    project.Members = null;

                    // remove ticket references from project
                    project.Tickets = null;

                    // remove tickets from DB & save chagens
                    this._ticketContext.tickets.Remove(ticket);
                    this._ticketContext.SaveChanges();
                });

                // remove prject reference from user who created it
                User projectCreator = _userContext.users.Find(project.CreatorID);
                projectCreator.CreatedProjects.Remove(project);

                // remove tickets from DB & save chagens
                this._projectContext.projects.Remove(project);
                this._projectContext.SaveChanges();
            });

            // remove company reference from user who created that company
            User companyCreator = await _userContext.users.FindAsync(company.CreatorID);
            companyCreator.Companies.Remove(company);
            await _userContext.SaveChangesAsync();

            // remove company from DB & save
            _companyContext.companies.Remove(company);
            await _companyContext.SaveChangesAsync();                     

            return NoContent();
        }

        // Remove project from company
        // PUT: api/Company
        [HttpPut("{companyId/projectId}")]
        public ActionResult<Company> RemoveProject(string copmanyId, string projectId)
        {
            Company company = _companyContext.companies.Find(copmanyId);
            Project project = company.Projects.Find(proj => proj.ProjectID == projectId);

            RemoveProjectMembers(project);
            RemoveTicketsFromProjectCreator(project);
            project.Members = null;
            RemoveTicketFromProject(project);

            company.Projects.Remove(project);
            _companyContext.SaveChanges();

            _projectContext.projects.Remove(project);

            return company;
        }

        private void RemoveProjectMembers(Project project)
        {
            project.Members.ForEach(delegate (User member)
            {
                member.AssignedProjects.Remove(project);

                member.AssignedTickets.ForEach(delegate (Ticket ticket) {
                    if (ticket.ProjectID == project.ProjectID)
                    {
                        _ = member.AssignedTickets.Remove(ticket);
                    }
                });
            });
        }

        private void RemoveTicketsFromProjectCreator(Project project)
        {
            User user = _userContext.users.Find(project.CreatorID);
            user.CreatedTickets.ForEach(delegate (Ticket ticket) {
                if (ticket.ProjectID == project.ProjectID)
                {
                    _ = user.CreatedTickets.Remove(ticket);
                }
            });

            user.CreatedProjects.Remove(project);
            _userContext.SaveChangesAsync();
        }

        private void RemoveTicketFromProject(Project project)
        {
            project.Tickets.ForEach(delegate (Ticket ticket) {
                _ticketContext.tickets.Remove(ticket);
            });

            _ticketContext.SaveChangesAsync();

            project.Tickets = null;
        }

        // Change company data by its id
        // PUT: api/Company
        [HttpDelete("{id}")]
        public async Task<ActionResult> ChangeCompany(string companyID, Company newCompany)
        {
            if (companyID != newCompany.CompanyID)
            {
                return BadRequest();
            }

            _companyContext.Entry(newCompany).State = EntityState.Modified;

            try
            {
                await _companyContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(companyID))
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

        // Get company by id
        // GET: api/Company
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompanyById(string companyID) => await _companyContext.companies.FindAsync(companyID);


        private bool CompanyExists(string id)
        {
            return _companyContext.companies.Any(e => e.CompanyID == id);
        }
    }
}
