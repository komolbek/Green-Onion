using System.Collections.Generic;
using System.Linq;
using GreenOnion.Server.DataLayer.DomainModels;

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
    }
}

        