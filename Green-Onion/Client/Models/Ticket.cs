using System;
namespace GreenOnion.Client.Models
{
    public class Ticket
    {
        public Ticket()
        {
        }

        public string ticketId { get; set; }
        public string projectId { get; set; }
        public string creatordId { get; set; }
        public string assigneeId { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public string complexity { get; set; }
        public DateTime startedDate { get; set; }
        public DateTime closedDate { get; set; }
        public DateTime dueDate { get; set; }
    }
}
