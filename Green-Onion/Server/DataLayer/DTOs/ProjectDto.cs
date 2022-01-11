using System.Collections.Generic;
using System;
using GreenOnion.Server.DataLayer.DomainModels;

namespace GreenOnion.Server.DataLayer.DTOs
{
    public class ProjectDto
    {
        public ProjectDto()
        {
        }

        private string projectId;

        private string companyId;

        private string userId;

        private List<Ticket> tickets;

        private List<User> members;

        private string name;

        private DateTime startedDate;

        private DateTime closedDate;
        private DateTime dueDate;
    }
}
