using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace JokeMoke.Client.Services.CommentService
{
    public class CommentService : ICommentService
    {
        public List<Joke> Jokes { get; set; } = new List<Joke>();
        public List<JokeType> JokeTypes { get; set; } = new List<JokeType>();
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public int JokeId { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }

        public string Message { get; set; }

        private HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public CommentService()
        {

        }

        public CommentService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public async Task GetComments(int id)
        {
            var result = await _http.GetFromJsonAsync<List<Comment>>($"joke/comments/{id}");
            if (result != null)
            {
                Comments = result;
            }
        }

        public async Task CreateComment(Comment comment, int id)
        {
            User currentUser = await _http.GetFromJsonAsync<User>("user/getcurrentuser");

            if (currentUser.Id == 0)
            {
                currentUser = null;
            }

            if (currentUser == null)
            {
                this.Message = "اول وارد شوید";
                _navigationManager.NavigateTo("/login");
            }
            else
            {
                comment.CreatedAt = DateTime.UtcNow;
                comment.CreatedBy = currentUser.Id;
                comment.JokeId = id;
                comment.IsApproved = false;
                this.Message = "کامنت ساخته شد";

                var result = await _http.PostAsJsonAsync("joke/comments", comment);
                await SetComments(result);
            }
        }

        private async Task SetComments(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Comment>>();
            Comments = response;
            _navigationManager.NavigateTo("/", true);
        }

        public static implicit operator CommentService(Comment comment)
        {
            return new CommentService
            {
                JokeId = comment.JokeId,
                Value = comment.Value,
                CreatedAt = comment.CreatedAt,
                CreatedBy = comment.CreatedBy,
                IsApproved = comment.IsApproved
            };
        }

        public static implicit operator Comment(CommentService commentService)
        {
            return new Comment
            {
                JokeId = commentService.JokeId,
                Value = commentService.Value,
                CreatedAt = commentService.CreatedAt,
                CreatedBy = commentService.CreatedBy,
                IsApproved = commentService.IsApproved
            };
        }
    }
}
