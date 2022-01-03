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
    }
}
