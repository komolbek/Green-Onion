using System.Collections.Generic;
using System;

namespace GreenOnion.Client.Models
{
    public class Project
    {
        public Project()
        {
        }

        public string projectId { get; set; }
        public string companyId { get; set; }
        public string creatorId { get; set; }
        public string name { get; set; }
        public List<Ticket> tickets { get; set; }
        public DateTime startedDate { get; set; }
        public DateTime closedDate { get; set; }
        public DateTime dueDate { get; set; }
        public List<User> members { get; set; }
    }

    // used as body object in get closed Projects within selected date range request.
    public class ProjectRange
    {
        private DateTime startDate;
        private DateTime endDate;
    }
}
