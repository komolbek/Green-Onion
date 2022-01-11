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
        public string userId { get; set; }

        [Required]
        public string firstName { get; set; }

        public string lastName { get; set; }

        public string aboutMe { get; set; }

        public string jobPosition { get; set; }
    }
}
