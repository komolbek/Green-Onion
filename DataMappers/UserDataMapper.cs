using System;
using GreenOnion.DomainModels;

namespace GreenOnion.DataMappers
{
    public class UserDataMapper
    {
        public UserDataMapper()
        {
        }

        public User Select(string userID)
        {
            return new User();
        }
    }
}
