//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Mvc;
//using System.Linq;
//using System.Threading.Tasks;
//using GreenOnion.Server.DataLayer.DomainModels;

//namespace GreenOnion.Server.Controllers
//{
//    [Route("api/tickets")]
//    [ApiController]
//    public class TicketsController : ControllerBase
//    {
//        private readonly GreenOnionContext _context;

//        public TicketsController(GreenOnionContext context)
//        {
//            this._context = context;
//        }

//        // Updates ticket data
//        // PUT: api/tickets
//        [Route("{ticketId}")]
//        [HttpPut]
//        public async Task<ActionResult<Ticket>> ChangeTicket(string ticketId, Ticket newTicket)
//        {

//            if (ticketId != newTicket.ticketId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(newTicket).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!TicketExists(ticketId))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // Gets ticket by id
//        // GET: api/tickets
//        [HttpGet("{ticketId}")]
//        public async Task<ActionResult<Ticket>> GetTicketById(string ticketId) => await _context.tickets.FindAsync(ticketId);

//        // Gets ticket assignee by ticket id
//        // GET: api/Ticket
//        [HttpGet]
//        [Route("{ticketId}")]
//        public async Task<ActionResult<User>> GetAssignee(string ticketID)
//        {
//            Ticket ticket = await _context.tickets.FindAsync(ticketID);
//            User ticketAssignee = await _context.users.FindAsync(ticket.userId);

//            return ticketAssignee;
//        }

//        // Assigns user to the ticket
//        // PUT: api/tickets
//        [Route("{ticketId}/{assigneeId}")]
//        [HttpPut]
//        public async Task<ActionResult<Ticket>> AssignUser(string ticketId, string assigneeId)
//        {
//            Ticket ticket = await _context.tickets.FindAsync(ticketId);
//            User ticketAssignee = await _context.users.FindAsync(ticket.userId);

//            ticket.userId = assigneeId;
//            _context.users.Add(ticketAssignee);

//            await _context.SaveChangesAsync();

//            return ticket;
//        }

//        // Removes user from the ticket
//        // PUT: api/tickets
//        [Route("{ticketId}/{assigneeId}")]
//        [HttpPut]
//        public async Task<ActionResult<Ticket>> RemoveAssignee(string ticketId)
//        {
//            Ticket ticket = await _context.tickets.FindAsync(ticketId);
//            User ticketAssignee = await _context.users.FindAsync(ticket.userId);

//            ticket.userId = null;
//            ticketAssignee.Tickets.Remove(ticket);

//            await _context.SaveChangesAsync();

//            return ticket;
//        }

//        private bool TicketExists(string id)
//        {
//            return _context.tickets.Any(e => e.ticketId == id);
//        }
//    }
//}
