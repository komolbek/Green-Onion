using GreenOnion.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using GreenOnion.Server.DataLayer.DataAccess;
using GreenOnion.Server.Datalayer.Dataaccess;
using System.Linq;
using System.Collections.Generic;

namespace GreenOnion.Server.Controllers
{
    [Route("api/ProjectsController")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {

        private readonly ProjectDbContext _context;
        private readonly UserDbContext _userContext;
        private readonly CompanyDbContext _companyContext;
        private readonly TicketDbContext _ticketContext;

        public ProjectsController(ProjectDbContext context, UserDbContext userDbContext, CompanyDbContext companyContext, TicketDbContext ticketContext)
        {
            this._context = context;
            this._userContext = userDbContext;
            this._companyContext = companyContext; 
            this._ticketContext = ticketContext; 
        }

        // Create a new project
        // POST: api/Project
        [HttpPost]
        [Route("{creatorId}/{companyId}")]
        public async Task<ActionResult<Project>> CreateProject(string creatorId, string companyId, Project project)
        {
            _context.projects.Add(project);
            await _context.SaveChangesAsync();

            Company company = await _companyContext.companies.FindAsync(companyId);
            company.Projects.Add(project);
            await _companyContext.SaveChangesAsync();

            User user = await _userContext.users.FindAsync(creatorId);
            user.CreatedProjects.Add(project);
            await _userContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProjectById), new { id = project.ProjectID}, project);
        }

        // Get project by its id
        // GET: api/Project
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProjectById(string projID)
        {            
            return await _context.projects.FindAsync(projID);
        }

        // Change project's data by its id
        // PUT: api/Project
        [HttpPut("{id}")]
        public async Task<ActionResult<Project>> ChangeProject(string projID, Project newPoject)
        {
            if (projID != newPoject.ProjectID)
            {
                return BadRequest();
            }

            _context.Entry(newPoject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(projID))
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

        // Add memeber to the project. Gets User & Project from DB by IDs, adds User to Project and saves Project records in the DB
        // PUT: api/Project
        [HttpPut("{projectId}")]
        public async Task<ActionResult<Project>> AddMember(string projId, User member)
        {
            Project project = await _context.projects.FindAsync(projId);
            project.Members.Add(member);
            await _context.SaveChangesAsync();

            _userContext.users.Add(member);            
            await _userContext.SaveChangesAsync();

            return project;
        }

        // Get project members. Members who have assigned Ticket from selected project
        // GET: api/Project
        [HttpGet("{id}")]
        public async Task<ActionResult<List<User>>> GetMembers(string projID)
        {
            Project project = await _context.projects.FindAsync(projID);

            return project.Members;
        }

        // Get project tickets
        // GET: api/Project
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Ticket>>> GetTickets(string projID)
        {
            Project project = await _context.projects.FindAsync(projID);

            return project.Tickets;
        }

        // Gets Ticket & Project from DB by IDs, adds Ticket to Project and saves Project records in the DB
        // PUT: api/Project
        [HttpPut("{projectId}")]
        public async Task<ActionResult<Project>> AddTicket(string projId, Ticket ticket)
        {
            Project project = await _context.projects.FindAsync(projId);
            project.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            _ticketContext.tickets.Add(ticket);
            await _ticketContext.SaveChangesAsync();

            return project;
        }

        // Remove ticket from project
        // UPDATE: api/Project
        [HttpPut("{projectId}/{ticketId}")]
        public async Task<ActionResult<Project>> RemoveProject(string projectId, string ticketId)
        {
            // TODO: implemeted deleting Ticket entity from DB as well. Now it's just removing it from list I guess.

            Project project = await _context.projects.FindAsync(projectId);
            Ticket ticket = project.Tickets.Find(tick => tick.TicketID == ticketId);

            project.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return project;
        }

        // Moves ticket in project list by changign ticket status in selected project & updates DB.
        // PUT: api/project
        [HttpPut("{projectId}/{ticketId}")]
        public async Task<ActionResult<Dictionary<string, List<Ticket>>>> MoveTicket(string projectId, string ticketId, string newTicketStatus, string oldTicketStatus)
        {
            Project project = await _context.projects.FindAsync(projectId);
            Ticket ticket = project.Tickets.Find(_tickId => _tickId.TicketID == ticketId);

            ticket.Status = newTicketStatus;
            await _context.SaveChangesAsync();

            return FilterProjectTicketsByStatus(project);
        }

        // Returns filtered Tickets by status.
        private Dictionary<string, List<Ticket>> FilterProjectTicketsByStatus(Project project)
        {
            Dictionary<string, List<Ticket>> filteredTicketsByProjectLists = new Dictionary<string, List<Ticket>>();

            foreach (Ticket ticket in project.Tickets)
            {
                if (ticket.Status == "todo")
                {
                    filteredTicketsByProjectLists["todo"].Add(ticket);
                }
                else if (ticket.Status == "doing")
                {
                    filteredTicketsByProjectLists["doing"].Add(ticket);
                }
                else
                {
                    filteredTicketsByProjectLists["done"].Add(ticket);
                }
            }

            return filteredTicketsByProjectLists;
        }

        private bool ProjectExists(string id)
        {
            return _context.projects.Any(e => e.ProjectID == id);
        }
    }
}
