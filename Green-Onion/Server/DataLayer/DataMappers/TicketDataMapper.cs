using System.Collections.Generic;
using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.DataLayer.DTOs;

namespace GreenOnion.Server.DataLayer.DataMappers
{
    public class TicketDataMapper
    {
        public TicketDataMapper()
        {
        }

        public static Ticket MapDtoToEntity(TicketDto ticketDto)
        {
            return new Ticket();
        }

        // Mapper for simple list view
        public static TicketDto MapEntityToDto(Ticket ticketEntity)
        {
            return new TicketDto()
            {
                ticketId = ticketEntity.ticketId,
                name = ticketEntity.title,
                status = ticketEntity.status,
                description = ticketEntity.description,
                complexity = ticketEntity.complexity,
                startedDate = ticketEntity.createdDate,
                closedDate = ticketEntity.closedDate,
                dueDate = ticketEntity.dueDate
            };
        }
    }
}
