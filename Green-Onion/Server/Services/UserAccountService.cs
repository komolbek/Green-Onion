using GreenOnion.Server.DataLayer.RequestModels;
using GreenOnion.DomainModels;
using GreenOnion.Server.Datalayer.Dataaccess;

namespace GreenOnion.Services
{
    public class UserAccountService
    {

        private UserDbContext _userContext;

        public UserAccountService()
        {
            this._userContext = new UserDbContext();
            //this.validator = new Validator();
        }

        public bool Save(UserPostRequest user)
        {
            User _user = new User();
            _user.Username = user.Username;
            _user.Password = user.Password;
            _user.LastName = null;
            _user.Companies = null;
            _user.AssignedProjects = null;
            _user.AboutMe = null;
            _user.AssignedTickets = null;
            _user.CreatedProjects = null;
            _user.Name = null;

            return this.userDataMapper.Insert(user);
        }

        private bool CheckUserIdentity(string name, string password)
        {
            
            return true;
        }

        public User? GetUser(string name, string password)
        {
            User userToCheck = new User();
            userToCheck.Name = name;
            userToCheck.Password = password;

            User retrievedUser = this.userDataMapper.Select(userToCheck);

            if (retrievedUser != null)
            {
                return retrievedUser;
            }
            else
            {
                return null;
            }
        }

        public bool ChangeUserPassword(string newPassword)
        {
            return true;
        }

        public bool ChangeUser(string userID, string name, string aboutInfo, string lastName)
        {
            User newUserData = this.userDataMapper.SelectById(userID);
            newUserData.Name = name;
            newUserData.AboutMe = aboutInfo;
            newUserData.LastName = lastName;

            return this.userDataMapper.Update(newUserData); ;
        }

        public void LogOut()
        {
            
        }
    }
}
