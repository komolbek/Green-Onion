using System.Collections.Generic;
using GreenOnion.Server.DataLayer.DataMappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GreenOnion.Server.DataLayer.DomainModels;
using GreenOnion.Server.DataLayer.DataAccess;
using GreenOnion.Server.DataLayer.RequestModels;

namespace Green_Onion.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly UserAccountDataAccess _userAccountDataAccess;

        public UserController(UserDataAccess userDataAccess, UserAccountDataAccess userAccountDataAccess)
        {
            _userDataAccess = userDataAccess;
            _userAccountDataAccess = userAccountDataAccess;
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<User> GetUsers()
        {
            return _userDataAccess.SelectAll();
        }

        // GET: api/User/getUser/id/5
        [Route("getUser/id/{id}")]
        [HttpGet]
        public ActionResult<User> GetUser(string id)
        {
            var user = _userDataAccess.Select(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/changeUser/id/1
        [Route("changeUser/id/{id}")]
        [HttpPut]
        public ActionResult<User> PutUser(string id, User newUserData)
        {
            if (id != newUserData.userId)
            {
                return BadRequest();
            }

            try
            {
                return _userDataAccess.Update(id, newUserData);
                
            } catch (DbUpdateException)
            {
                throw;
            }
        }

        // Get user by username & password
        // GET: api/User/signin
        [Route("signin")]
        [HttpGet]
        public ActionResult<User> LoginUser(UserSignInRequest signInRequest)
        {
            var userId = _userAccountDataAccess.Select(signInRequest.username, signInRequest.password);

            if (userId is "" || userId is null)
            {
                return BadRequest();
            }

            var user = _userDataAccess.Select(userId);

            if (user is not null)
            {
                return user;

            } else
            {
                return NoContent();
            }            
        }

        // Used while registration. Creates User object & associated UserAccount object
        // that can be used after loggin out.
        // POST: api/User
        [HttpPost]
        public ActionResult<User> PostUser(UserSignUpRequest signUpRequest)
        {
            var user = UserDataMapper.MapSignUpRequestToUser(signUpRequest);
            var userAccount = UserDataMapper.MapSignUpRequestToUserAccount(signUpRequest);

            try
            {
                _userDataAccess.Insert(user);

                _userAccountDataAccess.Insert(userAccount);
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtAction(nameof(GetUser), new { id = user.userId }, user);
        }        
    }
}
