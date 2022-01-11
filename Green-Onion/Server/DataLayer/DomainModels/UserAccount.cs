using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenOnion.Server.DataLayer.DomainModels
{
    // UserAccount class/table is used to store user's login and registration data.
    // Allows to avoid adding extra data if the User class/table were used for the
    // same purpose
    public class UserAccount
    {
        public UserAccount()
        {
        }

        // indicates unique username connected to the user.
        // Main purpose is to check identidy while login.
        [Key]
        [Required]        
        public string username { get; set; }


        // indicates user password for sign in/up.
        [Required]
        public string password { get; set; }

        // indicates user who is assoiciated with username.
        // After logging in app will get user data by this userId
        // if username & password are exist in database.
        [Required]
        [ForeignKey("User"), Column(Order = 2)]
        public string userId { get; set; }
    }
}
