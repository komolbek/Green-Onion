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

        public bool CreateUser()
        {
            return true;
        }

        private bool CheckUserIdentity()
        {
            return true;
        }

        public User GetUser()
        {
            return new User();
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
