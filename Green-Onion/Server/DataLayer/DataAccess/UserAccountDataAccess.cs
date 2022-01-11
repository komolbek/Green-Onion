using System.Data.Common;
using System.Linq;
using GreenOnion.Server.DataLayer.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace GreenOnion.Server.DataLayer.DataAccess
{
    public class UserAccountDataAccess
    {
        private readonly GreenOnionContext _context;

        public UserAccountDataAccess(GreenOnionContext context)
        {
            this._context = context;
        }

        // INSERT
        // adds new user_account associated with user into the db
        public void Insert(UserAccount userAccount)
        {
            _context.User_account.Add(userAccount);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

       // SELECT
       // retrieves userId by username & password from db
       public string Select(string username, string password)
        {
            try
            {
                var userId = _context.User_account
                    .FirstOrDefault(userAcc => userAcc.username == username & userAcc.password == password)
                    .userId;

                if (userId is null)
                {
                    return "";
                } else
                {
                    return userId;
                }


            } catch (DbException)
            {
                throw;
            }            
        }
    }
}
