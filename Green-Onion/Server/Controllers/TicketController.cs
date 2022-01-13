using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.DataLayer.DTOs;
using GreenOnion.Server.DataLayer.DataAccess;
using GreenOnion.Server.DataLayer.DataMappers;

namespace Green_Onion.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketDataAccess _ticketData;
        private readonly UserDataAccess _userData;
        private readonly ProjectDataAccess _projectData;
        private readonly TicketAssigneeDataAccess _ticketAssigneeData;

        public TicketController(
            TicketDataAccess ticketData,
            UserDataAccess userData,
            ProjectDataAccess projectData,
            TicketAssigneeDataAccess ticketAssigneeData)
        {
            _ticketData = ticketData;
            _userData = userData;
            _projectData = projectData;
            _ticketAssigneeData = ticketAssigneeData;
        }

        // GET: api/Ticket
        [HttpGet]
        public ActionResult<IEnumerable<TicketDto>> GetTickets()
        {
            List<TicketDto> ticketDtos = new();
            List<Ticket> ticketentites= _ticketData.SelectAll();

            foreach (var ticketEntity in ticketentites)
            {
                ticketDtos.Add(TicketDataMapper.MapEntityToDto(ticketEntity));
            }

            return ticketDtos;
        }

        // GET: api/Ticket/getById/5
        [Route("getById/{id}")]
        [HttpGet]
        public ActionResult<TicketDto> GetTicket(string id)
        {
            var ticketEntity = _ticketData.Select(id);
            var project = ProjectDataMapper.MapEntityToDto(_projectData.Select(ticketEntity.projectId));
            var assingee = GetAssingee(id);
            var creator = UserDataMapper.MapEntityToDto(_userData.Select(ticketEntity.userId));

            if (project == null || creator == null || assingee == null)
            {
                return NotFound();
            }

            return TicketDataMapper.MapEntityToDto(ticketEntity, creator, project, assingee);
        }

        private UserDto GetAssingee(string ticketId)
        {
            var ticketAssignees = _ticketAssigneeData.SelectAllByTicketId(ticketId);

            foreach (var ticketAssignee in ticketAssignees)
            {
                if(ticketAssignee.ticketId == ticketId)
                {
                    return UserDataMapper.MapEntityToDto(_userData.Select(ticketAssignee.userId));
                }
            }

            return null;
        }

        // Changes data about Ticket
        // PUT: api/Ticket/changeById/5
        [Route("changeById/{id}")]
        [HttpPut]
        public ActionResult<TicketDto> PutTicket(string id, TicketDto ticketDto)
        {
            if (id != ticketDto.ticketId && ticketDto.creator.userId is null)
            {
                return BadRequest();
            }

            // saving Dto data to use when mapping back
            var project = ticketDto.project;
            var creator = ticketDto.creator;
            var assignee = ticketDto.assignee;

            Ticket ticketEntity = TicketDataMapper.MapDtoToEntity(ticketDto);

            try
            {
                return TicketDataMapper.MapEntityToDto(_ticketData.Update(id, ticketEntity), creator, project, assignee);
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        // POST: api/Ticket/assignee/2
        [Route("assignee/{assigneeId?}")]
        [HttpPost]
        public ActionResult<TicketDto> PostTicket(Ticket ticket, string assigneeId = null)
        {
            var creator = UserDataMapper.MapEntityToDto(_userData.Select(ticket.userId));
            var project = ProjectDataMapper.MapEntityToDto(_projectData.Select(ticket.projectId));

            try
            {                
                if (creator is not null && project is not null)
                {
                    var ticketEntity = _ticketData.Insert(ticket);

                    if (assigneeId is not null)
                    {
                        var assignee = UserDataMapper.MapEntityToDto(_userData.Select(assigneeId));

                        TicketAssignee ticketAssignee = new()
                        {
                            ticketId = ticket.ticketId,
                            userId = assigneeId
                        };

                        _ticketAssigneeData.Insert(ticketAssignee);

                        return TicketDataMapper.MapEntityToDto(ticket, creator, project, assignee);
                    } else
                    {
                        return TicketDataMapper.MapEntityToDto(ticket, creator, project);
                    }
                }
                else
                {
                    return NoContent();
                }
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        // PUT: api/Ticket/addAssignee/4toTicket/3
        [Route("addAssignee/{assigneeId}/toTicket/{ticketId}")]
        [HttpPut]
        public ActionResult<TicketDto> AddAssignee(string assigneeId, string ticketId)
        {
            if (assigneeId is null | ticketId is null)
            {
                return BadRequest();
            }

            TicketAssignee ticketAssignee= new()
            {
                ticketId = ticketId,
                userId = assigneeId
            };

            _ticketAssigneeData.Insert(ticketAssignee);

            return GetTicket(ticketId);
        }

        // DELETE: api/Ticket/deleteById/5
        [Route("deleteById/{id}")]
        [HttpDelete]
        public string DeleteTicket(string id)
        {
            // delete ticket and assignee relation data
            _ticketAssigneeData.DeleteColumn(id);
            _ticketData.Delete(id);

            if (_ticketData.Select(id) is null)
            {
                return "successfully deleted";
            } else
            {
                return "could not deleted";
            }
        }
    }
}