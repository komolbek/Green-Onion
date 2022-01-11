using System.Collections.Generic;
using GreenOnion.Server.DataLayer.DomainModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GreenOnion.Server.Enums;
using GreenOnion.Server.DataLayer.DTOs;

namespace GreenOnion.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly GreenOnionContext _context;      

        public ReportsController(GreenOnionContext context)
        {
            this._context = context;
        }

        // Returns closed Projects within selected date range
        // GET: api/Report
        [Route("{companyId}")]
        [HttpGet]
        public async Task<ActionResult<List<Project>>> GetClosedProjects(string companyId, ProjectRange projectRange)
        {
            Company company = await _context.companies.FindAsync(companyId);

            List<Project> closedProjects = new List<Project>();

            company.Projects.ForEach(delegate (Project project)
            {
                if (project.closedDate >= projectRange.StartDate || project.closedDate <= projectRange.EndDate)
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

        // Returns closed tickets of all companyprojects within selected date range
        // GET: api/Report
        [Route("{companyId}")]
        [HttpGet]        
        public async Task<ActionResult<List<Ticket>>> GetClosedTickets(string companyId, ProjectRange projectRange)
        {
            Company company = await _context.companies.FindAsync(companyId);

            List<Ticket> tickets = new List<Ticket>();

            company.Projects.ForEach(delegate (Project project) {
                project.Tickets.ForEach(delegate (Ticket ticket) {
                    if (ticket.closedDate >= projectRange.StartDate || ticket.closedDate <= projectRange.EndDate)
                    {
                        tickets.Add(ticket);
                    }
                });
            });

            if (tickets.Count > 0)
            {
                return tickets;
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
        // GET: api/Report
        [Route("{companyId}")]
        [HttpGet]
        public async Task<ActionResult<Dictionary<string, string>>> GetProjectsProgress(string companyId)
        {
            // get company object
            Company company = await _context.companies.FindAsync(companyId);

            int numOfTickets = 0;
            int completedTickets = 0;
            string projectName = "";

            // result object
            Dictionary<string, string> projectsProgress = new Dictionary<string, string>();

            company.Projects.ForEach(delegate (Project project)
            {
                numOfTickets = project.Tickets.Count;
                projectName = project.name;

                project.Tickets.ForEach(delegate (Ticket ticket)
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
            double result = completedTickets * 100 / numOfTickets;

            return $"Progress {result}%";
        }

        // returns Dictionary of project Members and the amount of tickets they accomplished.
        // {Member 1: 6,
        //  Member 2: 5,
        //  Member 3: 11}
        // It's a list of key value pairs. Where name is a key & tickets amount is a value.
        // GET: api/Report
        [HttpGet]
        [Route("{projectId}")]
        public async Task<ActionResult<Dictionary<string, int>>> GetUserProductivity(string projectID)
        {
            Dictionary<string, int> usersProductivity = new Dictionary<string, int>();

            var project = await _context.projects.FindAsync(projectID);

            for (var i = 0; i < project.Tickets.Count; i++)
            {
                if (project.Tickets[i].Status == TicketStatus.Done.ToString())
                {
                    if (project.Tickets[i].UserId is null)
                    {
                        continue;
                    }
                    else
                    {
                        Ticket ticket = await _context.tickets.FindAsync(project.Tickets[i].TicketId);
                        User assignee = await _context.users.FindAsync(ticket.userId);

                        if (usersProductivity.ContainsKey(assignee.firstName))
                        {
                            usersProductivity[assignee.firstName] = usersProductivity[assignee.firstName] + 1;
                        }
                        else
                        {
                            usersProductivity.Add(assignee.firstName, 1);
                        }
                    }
                }
            }
            // TODO: what if no completed tickets in project??              
            return usersProductivity;
        }
    }
}
