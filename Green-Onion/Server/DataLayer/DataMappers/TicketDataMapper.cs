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
            return new() 
                {
                    ticketId = ticketDto.ticketId,
                    title = ticketDto.name,
                    userId = ticketDto.creator.userId,
                    projectId = ticketDto.project.projectId,
                    status = ticketDto.status,
                    description = ticketDto.description,
                    complexity = ticketDto.complexity,
                    createdDate = ticketDto.createdDate,
                    closedDate = ticketDto.closedDate,
                    dueDate = ticketDto.dueDate
                };
        }

        // Mapper for simple list view
        public static TicketDto MapEntityToDto(
            Microsoft.AspNetCore.Mvc.ActionResult<Ticket> ticketEntity,
            UserDto creator = null,
            ProjectDto project = null,
            UserDto assignee = null)
        {
            return new()
            {
                ticketId = ticketEntity.Value.ticketId,
                name = ticketEntity.Value.title,
                status = ticketEntity.Value.status,
                description = ticketEntity.Value.description,
                complexity = ticketEntity.Value.complexity,
                createdDate = ticketEntity.Value.createdDate,
                closedDate = ticketEntity.Value.closedDate,
                dueDate = ticketEntity.Value.dueDate,
                creator = creator,
                project = project,
                assignee = assignee
            };
        }
    }
}
