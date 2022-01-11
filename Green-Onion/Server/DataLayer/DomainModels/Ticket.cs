using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenOnion.Server.DataLayer.DomainModels
{
    public class Ticket
    {
        public Ticket()
        {

        }

        [Key]
        [Required]
        public string ticketId { get; set; }

        [Required]
        [ForeignKey("Project"), Column(Order = 1)]
        public string projectId { get; set; }

        // Indicates the user who created the ticket. To get the ticket assignee use TicketAssignee model.
        [Required]
        [ForeignKey("User"), Column(Order = 2)]
        public string userId { get; set; }

        [Required]
        public string title { get; set; }

        [Required]
        public string status { get; set; }

        public string description { get; set; }
        public string complexity { get; set; }

        [Required]
        public string startecreatedDate { get; set; }

        public string closedDate { get; set; }
        public string dueDate { get; set; }
    }
}
