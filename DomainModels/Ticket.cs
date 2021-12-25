using System;
namespace GreenOnion.DomainModels
{
    public class Ticket
    {

        private Int64 ticketID { get; set; }
        private Int64 projectID { get; set; }
        private Int64 userID { get; set; }
        private string name { get; set; }
        private string status { get; set; }
        private string info { get; set; }
        private DateTime startedDate { get; set; }
        private DateTime closedDate { get; set; }
        private DateTime dueDate { get; set; }

        public Ticket()
        {

        }

        
    }
}
