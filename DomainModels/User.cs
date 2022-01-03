using System;
using System.Collections.Generic;

namespace GreenOnion.DomainModels
{
    public class User
    {
        public User()
        {
            
        }

        private string userID;
        private string name;
        private string lastName;
        private string aboutMe;
        private string password;
        private Company[] companies;
        private Project[] createdProjects;
        private Project[] assignedProjects;
        private List<Ticket>? assignedTickets;

        public string UserID { get => userID; set => userID = value; }
        public string Name { get => name; set => name = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string AboutMe { get => aboutMe; set => aboutMe = value; }
        public Company[] Companies { get => companies; set => companies = value; }
        public Project[] CreatedProjects { get => createdProjects; set => createdProjects = value; }
        public Project[] AssignedProjects { get => assignedProjects; set => assignedProjects = value; }
        public List<Ticket>? AssignedTickets { get => assignedTickets; set => assignedTickets = value; }
        public string Password { get => password; set => password = value; }
    }
}
