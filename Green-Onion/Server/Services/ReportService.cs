using System.Collections.Generic;
using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.Enums;
using GreenOnion.Server.DataLayer.DTOs;
using GreenOnion.Server.DataLayer.DataAccess;
using System;

namespace GreenOnion.Server.Services
{
    public class ReportService
    {
        private readonly ProjectDataAccess _projectData;
        private readonly TicketDataAccess _ticketData;
        private readonly UserDataAccess _userData;
        private readonly CompanyDataAccess _companyData;

        public ReportService(
            TicketDataAccess ticketData,
            ProjectDataAccess projectData,
            UserDataAccess userData,
            CompanyDataAccess companyData)
        {
            _projectData = projectData;
            _ticketData = ticketData;
            _userData = userData;
            _companyData = companyData;
        }

        // Returns closed Projects within selected date range
        public List<Project> GetClosedProjects(string companyId, ProjectRange projectRange)
        {
            List<Project> projectEntities = _projectData.SelectAllByCompanyId(companyId);

            List<Project> closedProjects = new List<Project>();

            foreach(var project in projectEntities)
            {
                if (project.closedDate is  null)
                {
                    continue;
                }
                DateTime closedDate = DateTime.ParseExact(project.closedDate, PredictionService.dateFormat,
                                       System.Globalization.CultureInfo.InvariantCulture);

                if (closedDate >= projectRange.StartDate || closedDate <= projectRange.EndDate)
                {
                    closedProjects.Add(project);
                }
            }

            if (closedProjects.Count > 0)
            {
                return closedProjects;
            }
            else
            {
                return null;
            }
        }

        // Returns closed tickets of all companyprojects within selected date range
        public List<Ticket> GetClosedTickets(string projectId, ProjectRange projectRange)
        {
            List<Ticket> ticketEntities = _ticketData.SelectAllByProjectId(projectId);

            List<Ticket> closedTickets = new List<Ticket>();

            ticketEntities.ForEach(delegate (Ticket ticket)
            {
                DateTime closedDate = DateTime.ParseExact(ticket.closedDate, PredictionService.dateFormat,
                                       System.Globalization.CultureInfo.InvariantCulture);

                if (closedDate >= projectRange.StartDate || closedDate <= projectRange.EndDate)
                {
                    closedTickets.Add(ticket);
                }
            });

            if (closedTickets.Count > 0)
            {
                return closedTickets;
            }
            else
            {
                return null;
            }
        }

        // returns Dictionary of Projects and their progress in percentage.
        // {Project 1: 65%,
        //  Project 2: 35%,
        //  Project 3: 15%}
        // It's a list of key value pairs. Where name is a key & progress is a value. O(n^2)
        public Dictionary<string, string> GetProjectsProgress(string companyId)
        {
            List<Project> projectEntities = _projectData.SelectAllByCompanyId(companyId);

            int numOfTickets = 0;
            int completedTickets = 0;
            string projectName = "";

            // result object
            Dictionary<string, string> projectsProgress = new Dictionary<string, string>();

            projectEntities.ForEach(delegate (Project project)
            {
                var projectTickets = _ticketData.SelectAllByProjectId(project.projectId);
                numOfTickets = projectTickets.Count;
                projectName = project.name;

                projectTickets.ForEach(delegate (Ticket ticket)
                {
                    if (ticket.status == TicketStatus.Done.ToString())
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
            if (completedTickets > 0)
            {
                double result = completedTickets * 100 / numOfTickets;
                return $"Progress {result}%";
            }

            return "No completed tickets so far";
            
        }

        // TODO: optimise to new solution as above methods.
        // returns Dictionary of project Members and the amount of tickets they accomplished.
        // {Member 1: 6,
        //  Member 2: 5,
        //  Member 3: 11}
        // It's a list of key value pairs. Where name is a key & tickets amount is a value.
        public Dictionary<string, int> GetUserProductivity(string projectID)
        {
            //    Dictionary<string, int> usersProductivity = new Dictionary<string, int>();

            //    var projectTickets = _ticketData.SelectAllByProjectId(projectID);

            //    for (var i = 0; i < projectTickets.Count; i++)
            //    {
            //        if (projectTickets[i].status == TicketStatus.Done.ToString())
            //        {
            //            if (projectTickets[i].userId is null)
            //            {
            //                continue;
            //            }
            //            else
            //            {
            //                Ticket ticket =  _context.tickets.FindAsync(project.Tickets[i].TicketId);
            //                User assignee = _context.users.FindAsync(ticket.userId);

            //                if (usersProductivity.ContainsKey(assignee.firstName))
            //                {
            //                    usersProductivity[assignee.firstName] = usersProductivity[assignee.firstName] + 1;
            //                }
            //                else
            //                {
            //                    usersProductivity.Add(assignee.firstName, 1);
            //                }
            //            }
            //        }
            //    }
            //    // TODO: what if no completed tickets in project??              
            //return usersProductivity;
            return new Dictionary<string, int>();
        }
    }
}
