using JokeMoke.Server;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JokeMoke.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokeController : ControllerBase
    {
        private readonly JokeMokeContext _context;

        private readonly ILogger<JokeController> _logger;

        public JokeController(ILogger<JokeMokeContext> logger, JokeMokeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetJokes()
        {
            var jokes = await _context.Jokes.ToListAsync();
            return Ok(jokes);
        }

        [HttpGet("joketypes")]
        public async Task<ActionResult<List<JokeType>>> GetJokeTypes()
        {
            var JokeTypes = await _context.JokeTypes.ToListAsync();
            return Ok(JokeTypes);
        }

        [HttpPost]
        public async Task<ActionResult<List<Joke>>> CreateJoke(Joke joke)
        {
            joke.JokeType = null;

            _context.Jokes.Add(joke);
            await _context.SaveChangesAsync();
            return Ok(await GetDbJokes());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Joke>>> DeleteJoke(int id)
        {
            var dbJoke = await _context.Jokes
                .Include(sh => sh.JokeType)
                .FirstOrDefaultAsync(sh => sh.Id == id);

            if (dbJoke == null)
            {
                return NotFound("Sorry, No User For You!");
            }

            _context.Jokes.Remove(dbJoke);

            await _context.SaveChangesAsync();

            return Ok(await GetJokes());
        }

        private async Task<List<Joke>> GetDbJokes()
        {
            return await _context.Jokes.ToListAsync();
        }
    }
}
