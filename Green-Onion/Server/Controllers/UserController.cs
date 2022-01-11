using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenOnion.Server;
using GreenOnion.Server.DataLayer.DomainModels;

namespace Green_Onion.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GreenOnionContext _context;

        public UserController(GreenOnionContext context)
        {
            _context = context;
        }

        // GET: api/User
        [Route("all")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Getusers()
        {
            return await _context.users.ToListAsync();
        }

        // GET: api/User/5
        [Route("getById/{id}")]
        [HttpGet]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            var user = await _context.users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        [Route("changeById/{id}")]
        [HttpPut]
        public async Task<ActionResult<User>> PutUser(string id, User newUser)
        {
            if (id != newUser.UserId)
            {
                return BadRequest();
            }

            _context.Entry(newUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return await _context.users.FindAsync(id);
        }

        // Get user by username
        // GET: api/users
        [Route("login")]
        [HttpGet]
        public async Task<ActionResult<User>> LoginUser(string username, string password)
        {
            User user = new User();

            await _context.users.ForEachAsync(delegate (User _user)
            {
                if (_user.Username.Equals(username)) {
                    user = _user;
                }
            });

            if (user is not null)
            {
                return user;
            } else
            {
                return NoContent();
            }            
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        // DELETE: api/User/5
        [Route("deleteById/{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _context.users.Any(e => e.UserId == id);
        }
    }
}
