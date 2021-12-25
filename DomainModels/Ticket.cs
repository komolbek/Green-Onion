using System;
using GreenOnion.Services;

namespace GreenOnion.DomainModels
{
    public class Ticket
    {

        private Int64 ticketID { get; set; }
        public Int64 TicketID
        {
            get
            {
                return this.ticketID;
            }
            set
            {
                IDToIntGenerator idGenerator = new IDToIntGenerator();

                this.projectID = idGenerator.Generate();
            }
        }

        private Int64 projectID { get; set; }
        public Int64 ProjectID
        {
            get
            {
                return this.projectID;
            }
            set
            {
                this.projectID = value;
            }
        }


        private Int64 userID { get; set; }
        public Int64 UserID
        {
            get
            {
                return this.userID;
            }
            set
            {
                this.userID = value;
            }
        }

        private string name { get; set; }
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        private string status { get; set; }
        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
            }
        }

        private string info { get; set; }
        public string Info
        {
            get
            {
                return this.info;
            }
            set
            {
                this.info = value;
            }
        }

        private DateTime startedDate { get; set; }
        public DateTime StartedDate
        {
            get
            {
                return this.startedDate;
            }
            set
            {
                this.startedDate = value;
            }
        }

        private DateTime closedDate { get; set; }
        public DateTime ClosedDate
        {
            get
            {
                return this.closedDate;
            }
            set
            {
                this.closedDate = value;
            }
        }

        private DateTime dueDate { get; set; }
        public DateTime DueDate
        {
            get
            {
                return this.dueDate;
            }
            set
            {
                this.dueDate = value;
            }
        }


        public Ticket()
        {

        }
              
    }
}
