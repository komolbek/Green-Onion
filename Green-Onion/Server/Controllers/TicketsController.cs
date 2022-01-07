using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using GreenOnion.Server.DataLayer.DataAccess;
using GreenOnion.Server.Datalayer.Dataaccess;
using GreenOnion.DomainModels;

namespace GreenOnion.Server.Controllers
{
    [Route("api/TicketsController")]
    [ApiController]
    public class TicketsController : ControllerBase
    {

        private readonly TicketDbContext _ticketContext;
        private readonly UserDbContext _userContext;
        private readonly ProjectDbContext _projectContext;

        public TicketsController(TicketDbContext ticketContext, UserDbContext userDbContext, ProjectDbContext projectContext)
        {
            this._ticketContext = ticketContext;
            this._userContext = userDbContext;
            this._projectContext = projectContext;
        }
        
        // Creates new ticket
        // POST: api/Ticket
        [HttpPost]
        [Route("{projId/creatorId}")]
        [Route("{projId/creatorId/assigneeId}")]
        public async Task<ActionResult<Ticket>> CreateTicket(string projId, string creatorId, string assigneeId, Ticket ticket)
        {
            // add ticket to the creator ticket list
            User ticketCreator = await _userContext.users.FindAsync(creatorId);
            ticketCreator.CreatedTickets.Add(ticket);

            // add ticket to the project ticket list
            Project project = await _projectContext.projects.FindAsync(projId);
            project.Tickets.Add(ticket);

            // add ticket to the assignee ticket list
            User ticketAssignee = await _userContext.users.FindAsync(assigneeId);
            ticketAssignee.AssignedTickets.Add(ticket);

            // add project to the ticket assignee's assigned project list
            ticketAssignee.AssignedProjects.Add(project);

            // add assigned user to the project members list            
            project.Members.Add(ticketAssignee);

            _ticketContext.tickets.Add(ticket);
            await _projectContext.SaveChangesAsync();
            await _userContext.SaveChangesAsync();
            await _ticketContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.TicketID }, ticket);
        }

        // Updates ticket data
        // PUT: api/Ticket
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<Ticket>> ChangeTicket(string ticketId, Ticket newTicket)
        {

            if (ticketId != newTicket.TicketID)
            {
                return BadRequest();
            }

            _ticketContext.Entry(newTicket).State = EntityState.Modified;

            try
            {
                await _ticketContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(ticketId))
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

        // Deletes ticket from DB
        // DELETE: api/Ticket
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<Ticket>> DeleteTicketById(string ticketID)
        {
            Ticket ticket = await _ticketContext.tickets.FindAsync(ticketID);

            // remove ticket from project
            Project project = await _projectContext.projects.FindAsync(ticket.ProjectID);
            project.Tickets.Remove(ticket);

            await _projectContext.SaveChangesAsync();

            // remove ticket from creator's ticket list
            User ticketCreator = await _userContext.users.FindAsync(ticket.CreatorID);
            ticketCreator.CreatedTickets.Remove(ticket);

            //remove ticket from assignee's ticket list
            User ticketAssignee = await _userContext.users.FindAsync(ticket.CreatorID);
            ticketCreator.AssignedTickets.Remove(ticket);

            await _userContext.SaveChangesAsync();

            //remove ticket from tickets DB
            _ticketContext.tickets.Remove(ticket);

            await _ticketContext.SaveChangesAsync();

            return NoContent();
        }

        // Gets ticket by id
        // GET: api/Ticket
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicketById(string ticketID) => await _ticketContext.tickets.FindAsync(ticketID);

        // Gets ticket assignee by ticket & assignee id
        // GET: api/Ticket
        [HttpGet]
        [Route("{ticketId}/{assigneeId}")]
        public async Task<ActionResult<User>> GetAssignee(string ticketID, string assigneeId)
        {
            Ticket ticket = await _ticketContext.tickets.FindAsync(ticketID);
            User ticketAssignee = await _userContext.users.FindAsync(ticket.AssigneeID);

            return ticketAssignee;
        }

        // Assigns user to the ticket
        // PUT: api/Ticket
        [HttpPut]
        [Route("{ticketId}/{assigneeId}")]
        public async Task<ActionResult<Ticket>> AssignUser(string ticketID, string assigneeID)
        {
            Ticket ticket = await _ticketContext.tickets.FindAsync(ticketID);
            User ticketAssignee = await _userContext.users.FindAsync(ticket.AssigneeID);

            ticket.AssigneeID = assigneeID;
            _userContext.users.Add(ticketAssignee);

            await _ticketContext.SaveChangesAsync();
            await _userContext.SaveChangesAsync();

            return ticket;
        }

        // Removes user from the ticket
        // PUT: api/Ticket
        [HttpPut]
        [Route("{ticketId}/{assigneeId}")]
        public async Task<ActionResult<Ticket>> RemoveMember(string ticketID, string assigneeID)
        {
            Ticket ticket = await _ticketContext.tickets.FindAsync(ticketID);
            User ticketAssignee = await _userContext.users.FindAsync(ticket.AssigneeID);
            Project project = await _projectContext.projects.FindAsync(ticket.ProjectID);

            ticket.AssigneeID = null;
            ticketAssignee.AssignedTickets.Remove(ticket);
            project.Members.Remove(ticketAssignee);

            await _projectContext.SaveChangesAsync();
            await _ticketContext.SaveChangesAsync();
            await _userContext.SaveChangesAsync();

            return ticket;
        }

        private bool TicketExists(string id)
        {
            return _ticketContext.tickets.Any(e => e.TicketID == id);
        }
    }
}
