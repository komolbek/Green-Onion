using System.Collections.Generic;

namespace GreenOnion.Server.DataLayer.DTOs
{
    public class ProjectDto
    {
        public ProjectDto()
        {

        }

        public string projectId { get; set; }

        public CompanyDto company { get; set; }

        public UserDto creator { get; set; }

        public string name { get; set; }

        public List<UserDto> members { get; set; }        

        public List<TicketDto> tickets { get; set; }

        public string startedDate { get; set; }

        public string closedDate { get; set; }

        public string dueDate { get; set; }
    }
}
