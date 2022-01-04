using System;
using GreenOnion.DataMappers;
using GreenOnion.DomainModels;
using GreenOnion.Utilities;

namespace GreenOnion.Services
{
    public class UserAccountService
    {

        private UserDataMapper userDataMapper;
        private User user;
        private Validator validator;

        public UserAccountService()
        {
            this.userDataMapper = new UserDataMapper();
            this.validator = new Validator();
        }

        public bool CreateUser(string name, string password)
        {
            User user = new User();
            user.Name = name;
            user.Password = password;

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
