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
        public async Task<ActionResult<List<Joke>>> GetJokes()
        {
            var jokes = await _context.Jokes.ToListAsync();
            return Ok(jokes);
        }

        [HttpGet("getapprovedjokes")]
        public async Task<ActionResult<List<Joke>>> GetApprovedJokes()
        {
            var approvedjokes = await _context.Jokes.Where(sh => sh.IsApproved == true).ToListAsync();
            return Ok(approvedjokes);
        }

        [HttpGet("getnotapprovedjokes")]
        public async Task<ActionResult<List<Joke>>> GetNotApprovedJokes()
        {
            var notApprovedJokes = await _context.Jokes.Where(sh => sh.IsApproved == false).ToListAsync();
            return Ok(notApprovedJokes);
        }

        [HttpGet("joketypes")]
        public async Task<ActionResult<List<JokeType>>> GetJokeTypes()
        {
            var JokeTypes = await _context.JokeTypes.ToListAsync();
            return Ok(JokeTypes);
        }

        [HttpGet("comments/{id}")]
        public async Task<ActionResult<List<Comment>>> GetComments(Guid id)
        {
            var Comments = await _context.Comments.Where(sh => sh.JokeId == id && sh.IsApproved == true).ToListAsync();
            return Ok(Comments);
        }

        [HttpGet("getallcomments")]
        public async Task<ActionResult<List<Comment>>> GetAllComments()
        {
            var allComments = await _context.Comments.ToListAsync();
            return Ok(allComments);
        }

        [HttpGet("getapprovedcomments")]
        public async Task<ActionResult<List<Comment>>> GetApprovedComments()
        {
            var approvedComments = await _context.Comments.Where(sh => sh.IsApproved == true).ToListAsync();
            return Ok(approvedComments);
        }

        [HttpGet("getnotapprovedcomments")]
        public async Task<ActionResult<List<Comment>>> GetNotApprovedComments()
        {
            var notApprovedComments = await _context.Comments.Where(sh => sh.IsApproved == false).ToListAsync();
            return Ok(notApprovedComments);
        }

        [HttpGet("myjokes/{id}")]
        public async Task<ActionResult<List<Joke>>> GetMyJokes(Guid id)
        {
            var MyJokes = await _context.Jokes.Where(sh => sh.CreatedBy == id).ToListAsync();
            return Ok(MyJokes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Joke>> GetSingleJoke(Guid id)
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
            var result = _context.Jokes.Where(h => h.IsApproved == true).Skip(offset).FirstOrDefault();
            if(result == null)
            {
                Joke nullJoke = new Joke();
                return Ok(nullJoke);
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("stat/{id}")]
        public async Task<ActionResult<JokeStatistics>> GetStat(Guid id)
        {
            var stat = await _context.JokeStatisticsList
                .Include(h => h.Joke)
                .FirstOrDefaultAsync(h => h.JokeId == id);
            stat.Joke = null;
            return Ok(stat);
        }

        [HttpPut("like/{id}/{no}")]
        public async Task<ActionResult<List<JokeStatistics>>> LikeJoke(Guid id, int no, User currentUser)
            {
            var stat = await _context.JokeStatisticsList
                .Include(h => h.Joke)
                .FirstOrDefaultAsync(h => h.JokeId == id);

            var statlog = await _context.JokeStatisticsLogsList.Include(h => h.JokeStatistics)
                .Where(h => h.JokeStatisticsId == stat.Id && h.CreatedBy == currentUser.Id)
                .FirstOrDefaultAsync();

            if (statlog == null)
            {
                JokeStatisticsLogs jokestatlog = new JokeStatisticsLogs();

                jokestatlog.CreatedAt = DateTime.UtcNow;
                jokestatlog.CreatedBy = currentUser.Id;
                jokestatlog.JokeStatisticsId = stat.Id;

                if (no == 1)
                {
                    if (jokestatlog.LogType == 1)
                    {
                        jokestatlog.LogType = null;
                        stat.LikeCount -= 1;
                    }
                    else
                    {
                        if (jokestatlog.LogType != 2 && jokestatlog.LogType == null)
                        {
                            jokestatlog.LogType = 1;
                            stat.LikeCount += 1;
                        }

                        if (jokestatlog.LogType == 2)
                        {
                            jokestatlog.LogType = 1;
                            stat.DislikeCount -= 1;
                            stat.LikeCount += 1;
                        }
                    }

                    _context.JokeStatisticsLogsList.Add(jokestatlog);
                    await _context.SaveChangesAsync();
                    return Ok(await GetDbJokes());
                }

                if (no == 2)
                {
                    if (jokestatlog.LogType == 2)
                    {
                        jokestatlog.LogType = null;
                        stat.DislikeCount -= 1;
                    }
                    else
                    {
                        if (jokestatlog.LogType != 1 && jokestatlog.LogType == null)
                        {
                            jokestatlog.LogType = 2;
                            stat.DislikeCount += 1;
                        }

                        if (jokestatlog.LogType == 1)
                        {
                            jokestatlog.LogType = 2;
                            stat.LikeCount -= 1;
                            stat.DislikeCount += 1;
                        }
                    }
                    
                    _context.JokeStatisticsLogsList.Add(jokestatlog);
                    await _context.SaveChangesAsync();
                    return Ok(await GetDbJokes());
                }
            }

            if (statlog != null)
            {
                if (no == 1)
                {
                    if (statlog.LogType == 1)
                    {
                        statlog.LogType = null;
                        stat.LikeCount -= 1;
                    }
                    else
                    {
                        if (statlog.LogType != 2 && statlog.LogType == null)
                        {
                            statlog.LogType = 1;
                            stat.LikeCount += 1;
                        }

                        if (statlog.LogType == 2)
                        {
                            statlog.LogType = 1;
                            stat.DislikeCount -= 1;
                            stat.LikeCount += 1;
                        }
                    }

                    await _context.SaveChangesAsync();
                    return Ok(await GetDbJokes());
                }

                if (no == 2)
                {
                    if (statlog.LogType == 2)
                    {
                        statlog.LogType = null;
                        stat.DislikeCount -= 1;
                    }
                    else
                    {
                        if (statlog.LogType != 1 && statlog.LogType == null)
                        {
                            statlog.LogType = 2;
                            stat.DislikeCount += 1;
                        }

                        if (statlog.LogType == 1)
                        {
                            statlog.LogType = 2;
                            stat.DislikeCount += 1;
                            stat.LikeCount -= 1;
                        }
                    }
                    
                    await _context.SaveChangesAsync();
                    return Ok(await GetDbJokes());
                }
            }

            return Ok(await GetDbJokes());
        }

        [HttpPost]
        public async Task<ActionResult<List<Joke>>> CreateJoke(Joke joke)
        {
            joke.JokeType = null;
            joke.JokeStatistics = null;
            _context.Jokes.Add(joke);
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
        public async Task<ActionResult<List<Joke>>> DeleteJoke(Guid id)
        {
            var dbStat = await _context.JokeStatisticsList
                .Include(sh => sh.Joke)
                .FirstOrDefaultAsync(sh => sh.JokeId == id);

            dbStat.Joke = null;

            var dbStatLog = await _context.JokeStatisticsLogsList
                .Include(sh => sh.JokeStatistics)
                .FirstOrDefaultAsync(sh => sh.JokeStatisticsId == dbStat.Id);

            dbStatLog.JokeStatistics = null;

            _context.JokeStatisticsLogsList.Remove(dbStatLog);
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
                return NotFound("هیچ جوکی پیدا نشد");
            }

            _context.Jokes.Remove(dbJoke);
            await _context.SaveChangesAsync();

            return Ok(await GetJokes());
        }

        [HttpDelete("deletecomment/{id}")]
        public async Task<ActionResult<List<Comment>>> DeleteComment(Guid id)
        {
            var dbComment = await _context.Comments
                .Include(sh => sh.Joke)
                .FirstOrDefaultAsync(sh => sh.Id == id);

            if (dbComment == null)
            {
                return NotFound("هیچ کامنتی پیدا نشد");
            }

            _context.Comments.Remove(dbComment);

            await _context.SaveChangesAsync();

            return Ok(await GetDbComments());
        }

        [HttpDelete("deletealljokes")]
        public async Task<ActionResult<List<Joke>>> DeleteAllJokes()
        {
            var dbJokes = await _context.NotApprovedJokes
                .Include(sh => sh.JokeType).ToListAsync();

            if (dbJokes.Count() == 0)
            {
                return NotFound("هیچ جوکی پیدا نشد");
            }

            foreach (var joke in dbJokes)
            {
                joke.JokeType = null;
                _context.Remove(joke);
            }

            await _context.SaveChangesAsync();

            return Ok(await GetDbJokes());
        }

        [HttpDelete("deleteallcomments")]
        public async Task<ActionResult<List<Comment>>> DeleteAllComments()
        {
            var dbComments = await _context.NotApprovedComments
                .Include(sh => sh.Joke).ToListAsync();

            if (dbComments.Count() == 0)
            {
                return NotFound("هیچ کامنتی پیدا نشد");
            }

            foreach (var comment in dbComments)
            {
                comment.Joke = null;
                _context.Remove(comment);
            }

            await _context.SaveChangesAsync();

            return Ok(await GetDbComments());
        }

        [HttpPut("approvejoke/{Id}")]
        public async Task ApproveJoke(Guid Id, [FromBody] Joke joke)
        {
            Joke jokeToApprove = await _context.Jokes.Where(u => u.Id == Id).FirstOrDefaultAsync();

            jokeToApprove.IsApproved = true;

            JokeStatistics jokeStatistics = new JokeStatistics();

            jokeStatistics.JokeId = jokeToApprove.Id;
            jokeStatistics.LikeCount = 0;
            jokeStatistics.DislikeCount = 0;
            jokeStatistics.Joke = null;

            _context.JokeStatisticsList.Add(jokeStatistics);
            await _context.SaveChangesAsync();
        }

        [HttpPut("approvecomment/{Id}")]
        public async Task ApproveComment(Guid Id, [FromBody] Comment comment)
        {
            Comment commentToApprove = await _context.Comment.Where(u => u.Id == Id).FirstOrDefaultAsync();

            commentToApprove.IsApproved = true;

            await _context.SaveChangesAsync();
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
