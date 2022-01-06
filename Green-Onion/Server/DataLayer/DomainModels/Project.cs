using System;
using GreenOnion.Services;
using System.Collections;
using System.Collections.Generic;

namespace GreenOnion.DomainModels
{
    public class Project
    {
        public Project()
        {

        }

        private string projectID;
        private string companyID;
        private string creatorID;
        private string name;
        private List<Ticket>? tickets = null;
        private DateTime startedDate;
        private DateTime closedDate;
        private DateTime? dueDate;
        private List<User> members = null;

        public string ProjectID { get => projectID; set => projectID = value; }
        public string CompanyID { get => companyID; set => companyID = value; }
        public string CreatorID { get => creatorID; set => creatorID = value; }
        public string Name { get => name; set => name = value; }
        public List<Ticket>? Tickets { get => tickets; set => tickets = value; }
        public DateTime StartedDate { get => startedDate; set => startedDate = value; }
        public DateTime ClosedDate { get => closedDate; set => closedDate = value; }
        public DateTime? DueDate { get => dueDate; set => dueDate = value; }
        public List<User> Members { get => members; set => members = value; }
    }
}
