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

        [HttpGet("comments/{id}")]
        public async Task<ActionResult<List<Comment>>> GetComments(int id)
        {
            var Comments = await _context.Comments.Where(sh => sh.JokeId == id).ToListAsync();
            return Ok(Comments);
        }

        [HttpGet("MyJokes/{id}")]
        public async Task<ActionResult<List<Joke>>> GetMyJokes(int id)
        {
            var MyJokes = await _context.Jokes.Where(sh => sh.CreatedBy == id).ToListAsync();
            return Ok(MyJokes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Joke>> GetSingleJoke(int id)
        {
            var joke = await _context.Jokes
                .Include(h => h.JokeType)
                .FirstOrDefaultAsync(h => h.Id == id);
            joke.JokeType = null;
            if (joke == null)
            {
                return NotFound("Sorry, No Hero here!");
            }
            return Ok(joke);
        }

        [HttpGet("Random")]
        public async Task<ActionResult<Joke>> GetRandomJoke()
        {
            int total = _context.Jokes.Count();
            Random r = new Random();
            int offset = r.Next(0, total);
            var result = _context.Jokes.Skip(offset).FirstOrDefault();
            return Ok(result);
        }

        [HttpGet("stat/{id}")]
        public async Task<ActionResult<JokeStatistics>> GetStat(int id)
        {
            var stat = await _context.JokeStatisticsList
                .Include(h => h.Joke)
                .FirstOrDefaultAsync(h => h.JokeId == id);
            stat.Joke = null;
            return Ok(stat);
        }

        [HttpPut("like/{id}/{no}")]
        public async Task<ActionResult<List<JokeStatistics>>> LikeJoke(int id, int no, int what)
        {
            var stat = await _context.JokeStatisticsList
                .Include(h => h.Joke)
                .FirstOrDefaultAsync(h => h.JokeId == id);

            if(no == 1)
            {
                stat.LikeCount += 1;
                await _context.SaveChangesAsync();
                return Ok(await GetDbJokes());
            }
            else
            {
                stat.DislikeCount += 1;
                await _context.SaveChangesAsync();
                return Ok(await GetDbJokes());
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<Joke>>> CreateJoke(Joke joke)
        {
            joke.JokeType = null;
            joke.JokeStatistics = null;
            _context.Jokes.Add(joke);
            await _context.SaveChangesAsync();

            JokeStatistics jokeStatistics = new JokeStatistics();

            jokeStatistics.JokeId = joke.Id;
            jokeStatistics.LikeCount = 0;
            jokeStatistics.DislikeCount = 0;
            jokeStatistics.Joke = null;

            _context.JokeStatisticsList.Add(jokeStatistics);
            await _context.SaveChangesAsync();

            return Ok(await GetDbJokes());
        }

        [HttpPost("comments")]
        public async Task<ActionResult<List<Comment>>> CreateComment(Comment comment)
        {
            comment.Joke = null;

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Ok(await GetDbComments());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Joke>>> DeleteJoke(int id)
        {
            var dbStat = await _context.JokeStatisticsList
                .Include(sh => sh.Joke)
                .FirstOrDefaultAsync(sh => sh.JokeId == id);

            dbStat.Joke = null;

            _context.JokeStatisticsList.Remove(dbStat);
            await _context.SaveChangesAsync();

            var dbComments = await _context.Comments
                .Include(sh => sh.Joke)
                .Where(c => c.JokeId == id).ToListAsync();

            if (dbComments.Count() != 0)
            {
                foreach (var comment in dbComments)
                {
                    comment.Joke = null;
                    _context.Remove(comment);
                }

                await _context.SaveChangesAsync();
            }

            var dbJoke = await _context.Jokes
                .Include(sh => sh.JokeType)
                .FirstOrDefaultAsync(sh => sh.Id == id);

            dbJoke.JokeType = null;

            if (dbJoke == null)
            {
                return NotFound("Error, Joke Not Found!");
            }

            _context.Jokes.Remove(dbJoke);
            await _context.SaveChangesAsync();

            return Ok(await GetJokes());
        }

        private async Task<List<Joke>> GetDbJokes()
        {
            return await _context.Jokes.ToListAsync();
        }

        private async Task<List<Comment>> GetDbComments()
        {
            return await _context.Comments.ToListAsync();
        }

        private async Task<List<JokeStatistics>> GetDbStats()
        {
            return await _context.JokeStatisticsList.ToListAsync();
        }
    }
}
