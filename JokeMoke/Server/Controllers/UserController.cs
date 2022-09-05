using JokeMoke.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorFullStackCrud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JokeMokeContext _context;

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<JokeMokeContext> logger, JokeMokeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<Role>>> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetSingleUser(Guid id)
        {
            var user = await _context.Users
                .Include(h => h.Role)
                .FirstOrDefaultAsync(h => h.Id == id);
            user.Role = null;
            if (user == null)
            {
                return NotFound("Sorry, No User here!");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> CreateUser(User user)
        {
            user.Role = null;
            user.Password = Utility.Encrypt(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(await GetDbUsers());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<User>>> DeleteUser(Guid id)
        {
            var dbUser = await _context.Users
                .Include(sh => sh.Role)
                .FirstOrDefaultAsync(sh => sh.Id == id);

            if (dbUser == null)
            {
                return NotFound("Sorry, No User For You!");
            }

            _context.Users.Remove(dbUser);

            await _context.SaveChangesAsync();

            return Ok(await GetUsers());
        }

        private async Task<List<User>> GetDbUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // Authentication Methods ---------------------------------

        [HttpPost("loginuser")]
        public async Task<ActionResult<User>> LoginUser(User user)
        {
            user.Password = Utility.Encrypt(user.Password);

            User loggedInUser = await _context.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefaultAsync();

            if (loggedInUser != null)
            {
                // create claim
                var claim = new Claim(ClaimTypes.Name, loggedInUser.Email);

                // create claimsIdentity
                var claimsIdentity = new ClaimsIdentity(new[] { claim }, "serverAuth");

                // create claimsPrincipal
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Sing in User
                await HttpContext.SignInAsync(claimsPrincipal);
            }

            return await Task.FromResult(loggedInUser);
        }

        [HttpGet("getcurrentuser")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            User currentUser = new User();

            if (User.Identity.IsAuthenticated)
            {
                var emailAddress = User.FindFirstValue(ClaimTypes.Name);
                currentUser = await _context.Users.Where(u => u.Email == emailAddress)
                    .Include(r => r.Role)
                    .FirstOrDefaultAsync();

                currentUser.Role.User = null;

            }

            return await Task.FromResult(currentUser);
        }

        [HttpGet("logoutuser")]
        public async Task<ActionResult<String>> LogOutUser()
        {
            await HttpContext.SignOutAsync();
            return "Loged Out!";
        }

        [HttpPut("updateprofile/{Id}")]
        public async Task<User> UpdateProfile(Guid Id, [FromBody] User user)
        {
            User userToUpdate = await _context.Users.Where(u => u.Id == Id).FirstOrDefaultAsync();

            userToUpdate.UserName = user.UserName;
            userToUpdate.Email = user.Email;
            userToUpdate.ProfilePicUrl = user.ProfilePicUrl;

            await _context.SaveChangesAsync();

            return await Task.FromResult(user);
        }

        [HttpGet("getprofile/{Id}")]
        public async Task<User> GetProfile(Guid Id)
        {
            return await _context.Users.Where(u => u.Id == Id).FirstOrDefaultAsync();
        }

    }
}
