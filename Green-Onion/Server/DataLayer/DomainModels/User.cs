using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GreenOnion.Server.DataLayer.DomainModels
{
    public class User
    {
        public User(/*string name, string password*/)
        {
            //this.name = name;
            //this.password = password;
        }

        [Key]
        [Required]
        private string userId;

        [Required]
        private string firstName;

        private string lastName;

        private string aboutMe;

        public string UserId { get => userId; set => userId = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string AboutMe { get => aboutMe; set => aboutMe = value; }
    }
}
