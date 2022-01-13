
using System.Collections.Generic;
using System.Linq;
using GreenOnion.Server.DataLayer.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace GreenOnion.Server.DataLayer.DataAccess
{
    public class TicketAssigneeDataAccess
    {
        private readonly GreenOnionContext _context;

        public TicketAssigneeDataAccess(GreenOnionContext context)
        {
            this._context = context;
        }

        // INSERT
        // adds new project_member associated with project and member into the db
        public void Insert(TicketAssignee ticketAssignee)
        {
            _context.Ticket_assignee.Add(ticketAssignee);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        // SELECT
        // retrieves & returns all ticketAss from db related to the particular ticket by its id
        // used to get project members
        public List<TicketAssignee> SelectAllByTicketId(string id)
        {
            return _context.Ticket_assignee
                .Where(tass => tass.ticketId == id)
                .Select(tass => tass).ToList();
        }

        // DELETE
        // deletes ticket_assignee rows associated with ticket id from the db
        public void DeleteColumn(string ticketId)
        {
            List<TicketAssignee> ticketAssignee = _context.Ticket_assignee
                .Where(t => t.ticketId == ticketId)
                .Select(t => t).ToList();

            foreach (var tickAss in ticketAssignee)
            {
                _context.Ticket_assignee.Remove(tickAss);
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
    }
}