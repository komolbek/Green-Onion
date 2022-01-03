using System;
using System.Collections.Generic;
using GreenOnion.DataMappers;
using GreenOnion.DomainModels;

namespace GreenOnion.Services
{
    public class ProjectManagerService
    {

        private ProjectDataMapper projectDataMapper;

        public ProjectManagerService(ProjectDataMapper projectDataMapper)
        {
            this.projectDataMapper = projectDataMapper;
        }

        public Project CreateProject(string name, string companyID, string creatorID, DateTime? dueDate, Ticket[]? tickets)
        {

            return new Project();
        }

        // generates current date to add to startDate attribute of Project while creating object.
        private DateTime generateStartDate()
        {
            return new DateTime();
        }

        public bool DeleteProject(string projID)
        {
            return true;
        }

        public Project GetProject(string projID)
        {
            return new Project();
        }

        public bool ChangeProject(string projID, string? name, DateTime? dueDate, Ticket[]? tickets)
        {
            return true;
        }

        public bool AddMember(string userID, string projID)
        {
            return true;
        }

        public User[] GetMembers(string projID)
        {
            return new User[]{};
        }

        public Ticket[] GetTickets(string projID)
        {
            return new Ticket[]{};
        }

        public bool AddTicket(string projID, string ticketID)
        {
            return true;
        }

        public bool RemoveTicket(string projID, string ticketID)
        {
            return true;
        }

        public bool MoveTicket(string projID, string ticketID, string toList)
        {
            return true;
        }

        public Dictionary<string, Ticket[]> GetTicketsSortedInProjectLists(string projID)
        {
            return new Dictionary<string, Ticket[]>();
        }
    }
}
