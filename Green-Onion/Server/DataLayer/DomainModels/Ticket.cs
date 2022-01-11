using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenOnion.Server.DataLayer.DomainModels
{
    public class Ticket
    {

        [Key]
        [Required]
        private string ticketId;

        [Required]
        [ForeignKey("User"), Column(Order = 0)]
        private string projectIt;

        [Required]
        [ForeignKey("User"), Column(Order = 1)]
        private string userId;

        [Required]
        private string name;

        private string status;
        private string description;
        private string complexity;

        [Required]
        private DateTime startedDate;

        private DateTime closedDate;
        private DateTime  dueDate;

        public string TicketId { get => ticketId; set => ticketId = value; }
        public string ProjectId { get => projectIt; set => projectIt = value; }
        public string Name { get => name; set => name = value; }
        public string Status { get => status; set => status = value; }
        public string Description { get => description; set => description = value; }
        public DateTime StartedDate { get => startedDate; set => startedDate = value; }
        public DateTime ClosedDate { get => closedDate; set => closedDate = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public string UserId { get => userId; set => userId = value; }
        public string Complexity { get => complexity; set => complexity = value; }

        public Ticket()
        {

        }
    }
}
