using System;
using System.Collections.Generic;
using GreenOnion.DataMappers;
using GreenOnion.DomainModels;

namespace GreenOnion.Services
{
    public class ProjectManagerService
    {

        private ProjectDataMapper projectDataMapper;
        private UserDataMapper userDataMapper;
        private TicketDataMapper ticketDataMapper;

        public ProjectManagerService()
        {
            this.projectDataMapper = new ProjectDataMapper();
            this.userDataMapper = new UserDataMapper();
            this.ticketDataMapper = new TicketDataMapper();
        }

        public Project CreateProject(string name, string companyID, string creatorID, DateTime? dueDate, List<Ticket>[]? tickets)
        {
            Project project = new Project();
            project.Name = name;
            project.CompanyID = companyID;
            project.CreatorID = creatorID;
            project.DueDate = (DateTime)dueDate;
            project.StartedDate = DateTime.Today;

            this.projectDataMapper.insert(project);

            return new Project();
        }

        public bool DeleteProject(string projID)
        {
            return this.projectDataMapper.deleteBy(projID);
        }

        public Project GetProject(string projID)
        {
            return projectDataMapper.Select(projID);
        }

        public bool ChangeProject(string projID, string? name, DateTime? dueDate, List<Ticket>[]? tickets)
        {
            Project project = this.projectDataMapper.Select(projID);
            project.Name = name;
            project.DueDate = dueDate;

            return this.projectDataMapper.Update(project);
        }

        // Gets User & Project from DB by IDs, adds User to Project and updates Project records in the DB
        public bool AddMember(string userID, string projID)
        {
            User userObj = this.userDataMapper.Select(userID);
            Project projectObj = this.projectDataMapper.Select(projID);

            projectObj.Members.Add(userObj);

            return this.projectDataMapper.Update(projectObj);
        }

        public List<User>? GetMembers(string projID)
        {
            List<User> members = this.projectDataMapper.Select(projID).Members;

            if (members.Count > 0)
            {
                return members;
            }
            else
            {
                return null;
            }
        }

        public List<Ticket>? GetTickets(string projID)
        {
            List<Ticket> tickets = this.projectDataMapper.Select(projID).Tickets;

            if (tickets.Count > 0)
            {
                return tickets;
            } else
            {
                return null;
            }
            
        }

        // Gets Ticket & Project from DB by IDs, adds Ticket to Project and updates Project records in the DB
        public bool AddTicket(string projID, string ticketID)
        {
            Ticket ticket = this.ticketDataMapper.Select(ticketID);
            Project project = this.projectDataMapper.Select(projID);

            project.Tickets.Add(ticket);          

            return this.projectDataMapper.Update(project);
        }

        // Gets Ticket & Project from DB by IDs, remotves Ticket from Project and updates Project records in the DB
        public bool RemoveTicket(string projID, string ticketID)
        {
            Ticket ticket = this.ticketDataMapper.Select(ticketID);
            Project project = this.projectDataMapper.Select(projID);

            project.Tickets.Remove(ticket);

            return this.projectDataMapper.Update(project);
        }

        // changes ticket's list by removing it from previous list using previousTicketStatus
        // and using ticket object's new status adds to the new list.
        public Dictionary<string, List<Ticket>> MoveTicket(Dictionary<string, List<Ticket>> projectTickets, Ticket ticket, string previousTicketStatus)
        {
            string newTicketList = ticket.Status;
            string oldTicketList = previousTicketStatus;

            projectTickets[newTicketList].Add(ticket);
            projectTickets[oldTicketList].Remove(ticket);

            return projectTickets;
        }

        public Dictionary<string, List<Ticket>> GetTicketsSortedInProjectLists(string projID)
        {
            Project project = this.projectDataMapper.Select(projID);
            Dictionary<string, List<Ticket>> filteredTicketsByProjectLists = new Dictionary<string, List<Ticket>>();

            foreach (Ticket ticket in project.Tickets)
            {
                if (ticket.Status == "todo")
                {
                    filteredTicketsByProjectLists["todo"].Add(ticket);
                } else if (ticket.Status == "doing")
                {
                    filteredTicketsByProjectLists["doing"].Add(ticket);
                } else
                {
                    filteredTicketsByProjectLists["done"].Add(ticket);
                }
            }

            return filteredTicketsByProjectLists;
        }
    }
}
