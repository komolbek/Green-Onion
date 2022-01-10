using System.Collections.Generic;

namespace GreenOnion.DomainModels
{
    public class User
    {
        public User(/*string name, string password*/)
        {
            //this.name = name;
            //this.password = password;
        }

        private string userID;
        private string username;
        private string name;
        private string lastName;
        private string aboutMe;
        private string password;
        private List<Company>  companies;
        private List<Project> createdProjects;
        private List<Ticket> createdTickets;
        private List<Project> assignedProjects;
        private List<Ticket> assignedTickets;

        public string UserID { get => userID; set => userID = value; }
        public string Username { get => username; set => username = value; }
        public string Name { get => name; set => name = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string AboutMe { get => aboutMe; set => aboutMe = value; }
        public List<Company> Companies { get => companies; set => companies = value; }
        public List<Project> CreatedProjects { get => createdProjects; set => createdProjects = value; }
        public List<Ticket> CreatedTickets { get => createdTickets; set => createdTickets = value; }
        public List<Project> AssignedProjects { get => assignedProjects; set => assignedProjects = value; }
        public List<Ticket> AssignedTickets { get => assignedTickets; set => assignedTickets = value; }
        public string Password { get => password; set => password = value; }
    }
}
