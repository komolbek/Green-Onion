using System;
using GreenOnion.Services;
using System.Collections;

namespace GreenOnion.DomainModels
{
    public class Project
    {
        public Project()
        {

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
                IDToIntGenerator idGenerator = new IDToIntGenerator();

                this.projectID = idGenerator.Generate();
            }
        }

        private Int64 companyID { get; set; }
        public Int64 CompanyID
        {
            get
            {
                return this.companyID;
            }
            set
            {
                this.companyID = value;
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

        private Ticket[] tickets { get; set; }
        public Ticket[] Tickets
        {
            get
            {
                return this.tickets;
            }
            set
            {
                this.tickets = value;
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

        private User[] assignees { get; set; }
        public User[] Assignees
        {
            get
            {
                return this.assignees;
            }
            set
            {
                this.assignees = value;
            }
        }
    }
}
