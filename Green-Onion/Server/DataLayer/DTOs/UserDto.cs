using System.Collections.Generic;

namespace GreenOnion.Server.DataLayer.DTOs
{
    public class UserDto
    {
        public UserDto()
        {

        }

        public string userId { get; set; }

        public List<ProjectDto> createdProjects { get; set; }

        public List<ProjectDto> addedProjects { get; set; }

        public CompanyDto company { get; set; }

        public List<TicketDto> createdTickets { get; set; }

        public List<TicketDto> assignedTickets { get; set; }

        public string username { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string aboutMe { get; set; }

        public string jobPosition { get; set; }
    }
}
