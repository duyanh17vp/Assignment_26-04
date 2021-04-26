using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repository;

namespace WebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IRepositoryWrapper _repository;
        private LibraryDBContext _context;
        public UserController(IRepositoryWrapper repository, LibraryDBContext context)
        {
            _repository = repository;
            _context = context;
        }
        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]AuthenticateModel model)
        {
            var result = _context.Users.SingleOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
            if (result == null)
                return StatusCode(403, "Username or password is incorrect");
            ClaimsIdentity identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, result.UserName.ToString()),
                new Claim(ClaimTypes.Role, result.Role)
            }, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Ok(result);
        }
        // Logout
        [Authorize]
        [HttpPost("logout")]        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Logged out!");
        }
        //
        [Authorize(Roles = Role.SuperUser)]
        [HttpGet]
        public IActionResult GetAllUsers() 
        { 
            try 
            { 
                var users = _repository.User.GetAllUsers(); 
                return Ok(users); 
            }
            catch (Exception) 
            {
                // throw ex;
                return StatusCode(500, "Internal server error");
            } 
        }
        //
        [Authorize(Roles = Role.SuperUser)]
        [HttpGet("{id}", Name = "UserById")]
        public ActionResult<string> GetUserById(int id)
        {
            try
            {
                var user = _repository.User.GetUserById(id);
                if (user == null)
                {
                    return NotFound();
                }else
                {
                    return Ok(user);
                }
            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = Role.SuperUser)]
        [HttpGet("{id}/requestorders")]
        public ActionResult<string> GetUserWithDetails(int id)
        {
            try
            {
                var user = _repository.User.GetUserWithDetails(id);
                if (user == null)
                {
                    return NotFound();
                }else
                {
                    return Ok(user);
                }
            }catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        //
        [Authorize(Roles = Role.SuperUser)]
        [HttpPost]
        public async Task<ActionResult<User>>CreateUser([FromBody]User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid user object sent from client.");
                }
                await _repository.User.Create(user);
                _repository.Save();
                return CreatedAtRoute("UserById", new { id = user.Id }, user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = Role.SuperUser)]
        [HttpPut("{id}")]
        public IActionResult PutUser(int id,[FromBody]User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("User object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid user object sent from client.");
                }
                var userEntity = _repository.User.GetUserById(id);
                if (userEntity == null)
                {
                    return NotFound();
                }
                userEntity.UserName = user.UserName;
                userEntity.Password = user.Password;
                userEntity.FullName = user.FullName;
                _repository.User.Update(userEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [Authorize(Roles = Role.SuperUser)]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var userEntity = _repository.User.GetUserById(id);
                if (userEntity == null)
                {
                    return NotFound();
                }
                _repository.User.Delete(userEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}