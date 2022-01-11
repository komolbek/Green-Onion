using System.Collections.Generic;
using System.Linq;
using GreenOnion.Server.DataLayer.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreenOnion.Server.DataLayer.DataAccess
{
    public class UserDataAccess
    {

        private readonly GreenOnionContext _context;

        public UserDataAccess(GreenOnionContext context)
        {
            this._context = context;
        }

        // INSERT
        // adds user to db
        public void Insert(User user)
        {
            _context.User.Add(user);

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
        // retrieves user by his id
        public User Select(string id)
        {
            return _context.User.FirstOrDefault(user => user.userId == id);
        }

        // SELECT
        // retrieves & returns all users from db
        public IEnumerable<User> SelectAll()
        {
            return _context.User.ToList();
        }

        // UPDATE
        // updates user information
        public ActionResult<User> Update(string id, User newUserData)
        {
            _context.Entry(newUserData).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();

                return Select(newUserData.userId);
            }
            catch (DbUpdateException)
            {
                if (!UserExists(id))
                {
                    return new NotFoundResult();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.userId == id);
        }
    }
}
