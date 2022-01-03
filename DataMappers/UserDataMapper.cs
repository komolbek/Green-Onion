using System;
using GreenOnion.DomainModels;

namespace GreenOnion.DataMappers
{
    public class UserDataMapper
    {
        public UserDataMapper()
        {
        }

        public bool Insert(User user)
        {
            return true;
        }

        public User SelectById(string userID)
        {
            return new User();
        }

        public User Select(User user)
        {
            return new User();
        }

        public bool Update(User user)
        {
            return true;
        }
    }
}
