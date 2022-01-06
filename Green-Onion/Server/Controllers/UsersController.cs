using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenOnion.Server.Datalayer.Dataaccess;
using GreenOnion.DomainModels;
using System.Linq;

namespace GreenOnion.Server.Controllers
{
    [Route("api/[UsersController]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _userContext;

        public UsersController(UserDbContext _userContext)
        {
            this._userContext = _userContext;
        }

        // Create user
        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _userContext.users.Add(user);
            await _userContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserID }, user);
        }

        // Get user by id
        // GET: api/User
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            User user = await _userContext.users.FindAsync(id);

            return user;
        }

        // Get user by username
        // GET: api/User
        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            User user = await _userContext.users.FindAsync(username);

            return user;
        }

        // Change user data
        // PUT: api/User
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> PutUser(string id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _userContext.Entry(user).State = EntityState.Modified;

            try
            {
                await _userContext.SaveChangesAsync();
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

            return NoContent();
        }        

        private bool UserExists(string id)
        {
            return _userContext.users.Any(e => e.UserID == id);

        }
    }
}
