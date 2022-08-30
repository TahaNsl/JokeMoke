//using JokeMoke.Server;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace JokeMoke.Server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class CommentController : ControllerBase
//    {
//        private readonly JokeMokeContext _context;

//        private readonly ILogger<CommentController> _logger;

//        public CommentController(ILogger<JokeMokeContext> logger, JokeMokeContext context)
//        {
//            _context = context;
//        }

//        [HttpGet("comments/{id}")]
//        public async Task<ActionResult<List<JokeType>>> GetComments(int id)
//        {
//            var Comments = await _context.Comments.Where(sh => sh.JokeId == id).ToListAsync();
//            return Ok(Comments);
//        }

//        [HttpPost("comments")]
//        public async Task<ActionResult<List<Comment>>> CreateComment(Comment comment)
//        {
//            comment.Joke = null;

//            _context.Comments.Add(comment);
//            await _context.SaveChangesAsync();
//            return Ok(await GetDbComments());
//        }

//        private async Task<List<Comment>> GetDbComments()
//        {
//            return await _context.Comments.ToListAsync();
//        }
//    }
//}
