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

        // INSERT
        // adds project to db
        public Ticket Insert(Ticket ticket)
        {
            _context.Ticket.Add(ticket);

            try
            {
                _context.SaveChanges();

                return Select(ticket.ticketId);
            }
            catch (DbUpdateException)
            {
                throw;
            }
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

        // SELECT
        // retrieves & returns all tickets from db
        public List<Ticket> SelectAll()
        {
            return _context.Ticket.ToList();
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

        // Deletes project's tickets and their assignees by project id
        // DELETE
        public void DeleteAllTicketsOfProject(string projectId)
        {
            // all tickets
            var tickets = _context.Ticket
                .Where(tick => tick.projectId == projectId)
                .Select(tick => tick);

            foreach (var tick in tickets)
            {
                // all ticket+assignee relationships by ticket id
                var ticketAssignees = _context.Ticket_assignee
                    .Where(t_ass => t_ass.ticketId == tick.ticketId)
                    .Select(t_ass => t_ass);

                foreach (var tickAss in ticketAssignees)
                {
                    // delete ticket+assignee relationships
                    _context.Ticket_assignee.Remove(tickAss);
                }

                // deelte ticket itself
                _context.Ticket.Remove(tick);
            }

            SaveChanges();
        }

        // DELETE
        public void Delete(string id)
        {
            var ticket = _context.Ticket.FirstOrDefault(t => t.ticketId == id);
            _context.Ticket.Remove(ticket);
            SaveChanges();            
        }

        private void SaveChanges()
        {
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

        