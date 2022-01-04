using System;
using GreenOnion.DataMappers;
using GreenOnion.DomainModels;

namespace GreenOnion.Services
{
    public class TicketManagerService
    {
        private TicketDataMapper ticketDataMapper;

        public TicketManagerService()
        {
            this.ticketDataMapper = new TicketDataMapper();
        }

        public bool CreateTicket()
        {
            return true;
        }

        public bool ChangeTicket()
        {
            return true;
        }

        public bool DeleteTicket()
        {
            return true;
        }

        public Ticket GetTicket()
        {
            return new Ticket();
        }

        public bool AssignMember()
        {
            return true;
        }

        public bool RemoveMember()
        {
            return true;
        }
    }
}
