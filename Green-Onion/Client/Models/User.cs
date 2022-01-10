using System.Collections.Generic;

namespace GreenOnion.Client.Models
{
    public class User
    {
        public User()
        {
        }

        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        //public string jobTitle { get; set; }
        public string userName { get; set; }
        //public string email { get; set; }
        public string password { get; set; }
        public string aboutMe { get; set; }
        public List<Company> companies { get; set; }
        public List<Project> createdProjects { get; set; }
        public List<Ticket> createdTickets { get; set; }
        public List<Project> assignedProjects { get; set; }
        public List<Ticket> assignedTickets { get; set; }
    }

    public class UserSignInRequest
    {
        public UserSignInRequest()
        {
        }

        public string userName { get; set; }
        public string password { get; set; }
    }

    public class UserSignUpRequest
    {
        public UserSignUpRequest()
        {
        }

        public string userId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string passwordConfirm { get; set; }
    }
}
