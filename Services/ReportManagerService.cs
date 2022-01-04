using System;
using System.Collections.Generic;
using GreenOnion.DomainModels;

namespace GreenOnion.Services
{
    public class ReportManagerService
    {
        private List<Project> projects;
        private ProjectManagerService projectManagerService;
        private TicketManagerService ticketManagerService;

        public ReportManagerService(List<Project> projects, ProjectManagerService projectManagerService)
        {
            this.projects = projects;
            this.projectManagerService = projectManagerService;
            this.ticketManagerService = new TicketManagerService();
    }

        // this method return closed Projects within selected date range
        public List<Project>? GetClosedProjects(DateTime startDate, DateTime endDate)
        {

            List<Project> closedProjects = new List<Project>();

            this.projects.ForEach(delegate(Project project)
            {
                if (project.ClosedDate >= startDate || project.ClosedDate <= endDate)
                {
                    closedProjects.Add(project);
                }
            });

            if (closedProjects.Count > 0)
            {
                return closedProjects;
            }
            else
            {
                return null;
            }           
        }

        // this method return closed Tickets within selected date range
        public List<Ticket>? GetClosedTickets(DateTime startDate, DateTime endDate)
        {

            List<Ticket> tickets = new List<Ticket>();

            this.projects.ForEach(delegate (Project project) {
                project.Tickets.ForEach(delegate(Ticket ticket) {
	                if (ticket.ClosedDate >= startDate || ticket.ClosedDate <= endDate)
                    {
                        tickets.Add(ticket);
                    }
        	    });
            });

            if (tickets.Count > 0)
            {
                return tickets;
            } else
            {
                return null;
            }
        }

        // returns Dictionary of Projects and their progress in percentage.
        // {Project 1: 65%,
        //  Project 2: 35%,
        //  Project 3: 15%}
        // It's a list of key value pairs. Where name is a key & progress is a value. O(n^2)
        public Dictionary<string, string> GetProjectsProgress()
        {
            int numOfTickets = 0;
            int completedTickets = 0;
            string projectName = "";

            //
            Dictionary<string, string> projectsProgress = new Dictionary<string, string>();

            projects.ForEach(delegate (Project project)
            {
                numOfTickets = project.Tickets.Count;
                projectName = project.Name;

                project.Tickets.ForEach(delegate (Ticket ticket)
                {
                    if (ticket.Status == TicketStatus.Done.ToString())
                    {
                        completedTickets += 1;
                    }
                });

                string progress = this.CalculateProgressPercetage(numOfTickets, completedTickets);
                projectsProgress[projectName] = progress;

                completedTickets = 0;
            });


            return projectsProgress;
        }

        private string CalculateProgressPercetage(int numOfTickets, int completedTickets)
        {
            double result = completedTickets * 100 / numOfTickets;

            return $"Progress {result}%"; 
        }

        // returns Dictionary of project Members and the amount of tickets they accomplished.
        // {Member 1: 6,
        //  Member 2: 5,
        //  Member 3: 11}
        // It's a list of key value pairs. Where name is a key & tickets amount is a value.
        public Dictionary<string, int> GetUserProductivity(string projectID)
        {
            Dictionary<string, int> usersProductivity = new Dictionary<string, int> ();

            Project project = this.projectManagerService.GetProject(projectID);

            for (int i = 0; i < project.Tickets.Count; i++)
            {
                if (project.Tickets[i].Status == TicketStatus.Done.ToString())
                {
                    if (project.Tickets[i].AssigneeID is null)
                    {
                        continue;
                    }
                    else
                    {
                        User assignee = this.ticketManagerService.GetAssignee(project.Tickets[i].TicketID);

                        if (usersProductivity.ContainsKey(assignee.Name))
                        {
                            usersProductivity[assignee.Name] = usersProductivity[assignee.Name] + 1;
                        } else
                        {
                            usersProductivity.Add(assignee.Name, 1);
                        }
                    }
                }
            }
                        
            return usersProductivity;
        }
    }
}
