//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using GreenOnion.Server;
//using GreenOnion.Server.DataLayer.DomainModels;

//namespace Green_Onion.Server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TicketController : ControllerBase
//    {
//        private readonly GreenOnionContext _context;

//        public TicketController(GreenOnionContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Ticket
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<Ticket>>> Gettickets()
//        {
//            return await _context.tickets.ToListAsync();
//        }

//        // GET: api/Ticket/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Ticket>> GetTicket(string id)
//        {
//            var ticket = await _context.tickets.FindAsync(id);

//            if (ticket == null)
//            {
//                return NotFound();
//            }

//            return ticket;
//        }

//        // PUT: api/Ticket/5
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutTicket(string id, Ticket ticket)
//        {
//            if (id != ticket.ticketId)
//            {
//                return BadRequest();
//            }

//            _context.Entry(ticket).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!TicketExists(id))
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

//        // POST: api/Ticket
//        [HttpPost]
//        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
//        {
//            _context.tickets.Add(ticket);
//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateException)
//            {
//                if (TicketExists(ticket.ticketId))
//                {
//                    return Conflict();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return CreatedAtAction("GetTicket", new { id = ticket.ticketId }, ticket);
//        }

//        // DELETE: api/Ticket/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteTicket(string id)
//        {
//            var ticket = await _context.tickets.FindAsync(id);
//            if (ticket == null)
//            {
//                return NotFound();
//            }

//            _context.tickets.Remove(ticket);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool TicketExists(string id)
//        {
//            return _context.tickets.Any(e => e.ticketId == id);
//        }
//    }
//}

// POST

// Gets Ticket & Project from DB by IDs, adds Ticket to Project and saves Project records in the DB
// PUT: api/projects
//[Route("addTicketByProjectId/{projectId}")]
//[HttpPut]
//public async Task<ActionResult<Project>> AddTicket(string projId, Ticket ticket)
//{
//    Project project = await _context.projects.FindAsync(projId);
//    project.Tickets.Add(ticket);

//    _context.tickets.Add(ticket);

//    await _context.SaveChangesAsync();

//    return project;
//}

//DELETE

// Removes ticket from project. Also deletes ticket from DB. Call this api to delete ticket.
//        // UPDATE: api/projects
//        [Route("removeTicket/{ticketId}/inProject/{projectId}")]
//        [HttpPut]
//        public async Task<ActionResult<Project>> RemoveTicket(string projectId, string ticketId)
//        {
//            Project project = await _context.projects.FindAsync(projectId);
//            Ticket ticket = project.Tickets.Find(tick => tick.TicketId == ticketId);

//            project.Tickets.Remove(ticket);

//            _context.tickets.Remove(ticket);

//            await _context.SaveChangesAsync();

//            return project;
//        }