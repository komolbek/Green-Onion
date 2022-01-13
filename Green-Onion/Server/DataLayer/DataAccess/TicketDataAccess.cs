using System.Collections.Generic;
using System.Linq;
using GreenOnion.Server.DataLayer.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenOnion.Server.DataLayer.DataAccess
{
    public class TicketDataAccess
    {
        private readonly GreenOnionContext _context;

        public TicketDataAccess(GreenOnionContext context)
        {
            this._context = context;
        }

        // SELECT
        // retrieves & returns all tickets related to particular project by its id
        public List<Ticket> SelectAllByProjectId(string id)
        {
            return _context.Ticket
                .Where(tick => tick.projectId == id)
                .Select(tick => tick).ToList();
        }

        // SELECT
        // retrieves ticket by his id
        public Ticket Select(string id)
        {
            return _context.Ticket.FirstOrDefault(t => t.ticketId == id);
        }

        // UPDATE
        // updates project information
        public ActionResult<Ticket> Update(string id, Ticket ticket)
        {
            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();

                return Select(ticket.ticketId);
            }
            catch (DbUpdateException)
            {
                if (!TicketExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE
        public void DeleteByColumnProjId(string projectId)
        {
            var tickets = _context.Ticket
                .Where(tick => tick.projectId == projectId)
                .Select(tick => tick);

            foreach (var tick in tickets)
            {
                var ticketAssignees = _context.Ticket_assignee
                    .Where(t_ass => t_ass.ticketId == tick.ticketId)
                    .Select(t_ass => t_ass);

                foreach (var tickAss in ticketAssignees)
                {
                    _context.Ticket_assignee.Remove(tickAss);
                }

                _context.Ticket.Remove(tick);
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        private bool TicketExists(string id)
        {
            return _context.Ticket.Any(e => e.ticketId == id);
        }
    }
}

        