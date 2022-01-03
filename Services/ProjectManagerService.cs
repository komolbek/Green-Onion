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

        public Project CreateProject()
        {

            return new Project();
        }

        public bool DeleteProject()
        {
            return true;
        }

        public Project GetProject()
        {
            return new Project();
        }

        public bool ChangeProject()
        {
            return true;
        }

        public bool AddMember()
        {
            return true;
        }

        public User[] GetMembers()
        {
            return new User[]{};
        }

        public Ticket[] GetTickets()
        {
            return new Ticket[]{};
        }

        public bool AddTicket()
        {
            return true;
        }

        public bool RemoveTicket()
        {
            return true;
        }

        public bool MoveTicketTo()
        {
            return true;
        }

        public Dictionary<string, Ticket[]> GetTicketsSortedInProjectLists()
        {
            return new Dictionary<string, Ticket[]>();
        }
    }
}
