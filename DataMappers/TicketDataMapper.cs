using System;
using GreenOnion.DomainModels;

namespace GreenOnion.DataMappers
{
    public class TicketDataMapper
    {
        public TicketDataMapper()
        {
        }

        public Ticket Select(string ticketID)
        {
            return new Ticket();
        }

        public bool Insert(Ticket ticket)
        {
            return true;
        }

        public bool Update(Ticket ticket)
        {
            return true;
        }

        public bool Delete(string ticketID)
        {
            return true;
        }

        public User SelectAssignee(string ticketID)
        {
            return new User();
        }

        public bool UpdateAssignee(Ticket ticket)
        {
            return true;
        }

        public bool RemoveAssignee(Ticket ticket)
        {
            return true;
        }
    }
}
