using System;
namespace GreenOnion.Server.DataLayer.DTOs
{
    public class TicketDto
    {
        public TicketDto()
        {
        }

        public string ticketId { get; set; }

        public ProjectDto project { get; set; }

        public UserDto creator { get; set; }

        public UserDto assignee { get; set; }

        public string name { get; set; }

        public string status { get; set; }

        public string description { get; set; }

        public string complexity { get; set; }

        public string createdDate { get; set; }

        public string closedDate { get; set; }

        public string dueDate { get; set; }

    }
}
