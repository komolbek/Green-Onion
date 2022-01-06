using System;
using GreenOnion.Services;

namespace GreenOnion.DomainModels
{
    public class Ticket
    {

        private string ticketID;
        private string projectID;
        private string creatorID;
        private string assigneeID;
        private string name;
        private string status;
        private string description;
        private string complexity;
        private DateTime startedDate;
        private DateTime? closedDate = null;
        private DateTime?  dueDate = null;

        public string TicketID { get => ticketID; set => ticketID = value; }
        public string ProjectID { get => projectID; set => projectID = value; }
        public string CreatorID { get => creatorID; set => creatorID = value; }
        public string Name { get => name; set => name = value; }
        public string Status { get => status; set => status = value; }
        public string Description { get => description; set => description = value; }
        public DateTime StartedDate { get => startedDate; set => startedDate = value; }
        public DateTime? ClosedDate { get => closedDate; set => closedDate = value; }
        public DateTime? DueDate { get => dueDate; set => dueDate = value; }
        public string AssigneeID { get => assigneeID; set => assigneeID = value; }
        public string Complexity { get => complexity; set => complexity = value; }

        public Ticket()
        {

        }
    }
}
