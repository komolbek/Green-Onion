using System;
using System.Collections.Generic;
using GreenOnion.DomainModels;

namespace GreenOnion.Services
{
    public class ReportManagerService
    {
        public ReportManagerService()
        {
        }

        public List<Project> GetClosedProjects()
        {
            return new List<Project>();
        }

        public List<Ticket> GetClosedTickets()
        {

            return new List<Ticket>();
        }

        // returns array of Projects and their progress in percentage.
        // {Project 1: 65%,
        //  Project 2: 35%,
        //  Project 3: 15%}
        // It's a list of key value pairs. Where name is a key & progress is a value.
        public Dictionary<string, string> GetProjectsProgress()
        {
            return new Dictionary<string, string>();
        }

        public Dictionary<string, string> GetUserProductivity()
        {
            return new Dictionary<string, string>();
        }
    }
}
