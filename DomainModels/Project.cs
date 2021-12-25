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
                return this.companyID;
            }
            set
            {
                this.companyID = value;
            }
        }

        private string name { get; set; }
        public Int64 Name
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

        private Ticket[] tickets { get; set; }
        public Int64 Tickets
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

        private DateTime startedDate { get; set; }
        public Int64 StartedDate
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

        private DateTime closedDate { get; set; }
        public Int64 ClosedDate
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

        private DateTime dueDate { get; set; }
        public Int64 DueDate
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

        private User[] assignees { get; set; }
        public Int64 Assignees
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
    }
}
