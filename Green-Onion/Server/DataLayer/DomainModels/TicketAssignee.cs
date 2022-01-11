using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenOnion.Server.DataLayer.DomainModels
{
    public class TicketAssignee
    {
        public TicketAssignee()
        {
        }

        // indicates ticket where user added as assignee.
        [Required]
        [ForeignKey("Ticket"), Column(Order = 0)]
        public string ticketId { get; set; }


        // indicates user who were assigned to the ticket as assingee.
        // Assigned Users who were not created a ticket still can delete or change it.
        // P.S. User who creates ticket wont becomes member automatically.
        [Required]
        [ForeignKey("User"), Column(Order = 1)]
        public string userId { get; set; }
    }
}
