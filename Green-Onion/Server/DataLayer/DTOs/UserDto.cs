using GreenOnion.Server.DataLayer.DomainModels;
using System.Collections.Generic;

namespace GreenOnion.Server.DataLayer.DTOs
{
    public class UserDto
    {
        public UserDto()
        {
        }

        private string userId;

        public string projectId { get; set; }

        public string companyId { get; set; }

        private List<Ticket> tickets;

        private string username;

        private string firstName;

        private string lastName;
        private string aboutMe;

        private string password;

        public string UserId { get => userId; set => userId = value; }
        public string Username { get => username; set => username = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string AboutMe { get => aboutMe; set => aboutMe = value; }
        public string Password { get => password; set => password = value; }
        public List<Ticket> Tickets { get => tickets; set => tickets = value; }
    }
}
