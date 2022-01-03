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
            User user = new User(name, password);
            
            return this.userDataMapper.Insert(user);
        }

        private bool CheckUserIdentity(string name, string password)
        {
            
            return true;
        }

        public User? GetUser(string name, string password)
        {
            User userToCheck = new User(name, password);

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

        public bool ChangeUser(string name, string password, string aboutInfo, string lastName)
        {
            return true;
        }

        public void LogOut()
        {
            
        }
    }
}
