using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace JokeMoke.Client.Services.CommentService
{
    public class CommentService : ICommentService
    {
        public List<Joke> Jokes { get; set; } = new List<Joke>();
        public List<JokeType> JokeTypes { get; set; } = new List<JokeType>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Comment> AllComments { get; set; } = new List<Comment>();

        public List<Comment> ApprovedComments { get; set; } = new List<Comment>();
        public List<Comment> NotApprovedComments { get; set; } = new List<Comment>();

        public Guid JokeId { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
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

        public async Task GetAllComments()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<Comment>>("joke/getallcomments");
                if (result != null && result.Count() != 0)
                {
                    AllComments = result;
                }
                else
                {
                    throw new Exception("هیچ جوکی یافت نشد");
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }

        }

        public async Task GetComments(Guid id)
        {
            var result = await _http.GetFromJsonAsync<List<Comment>>($"joke/comments/{id}");
            if (result != null)
            {
                Comments = result;
            }
        }

        public async Task GetApprovedComments()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<Comment>>("joke/getapprovedcomments");
                if (result != null && result.Count() != 0)
                {
                    ApprovedComments = result;
                }
                else
                {
                    throw new Exception("هیچ نظری یافت نشد");
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public async Task GetNotApprovedComments()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<Comment>>("joke/getnotapprovedcomments");
                if (result != null && result.Count() != 0)
                {
                    NotApprovedComments = result;
                }
                else
                {
                    throw new Exception("هیچ نظری یافت نشد");
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public async Task CreateComment(Comment comment, Guid id)
        {
            User currentUser = await _http.GetFromJsonAsync<User>("user/getcurrentuser");

            if (currentUser.Id == Guid.Empty)
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

        public async Task DeleteComment(Guid id)
        {
            try
            {
                await _http.DeleteAsync($"joke/deletecomment/{id}");
            }
            catch (Exception ex)
            {

                _ = ex.Message;
            }

        }

        public async Task ApproveComment(Guid id)
        {
            Comment comment = new Comment();

            await _http.PutAsJsonAsync($"joke/approvecomment/{id}", comment);
        }

        public async Task DeleteAllComments()
        {
            try
            {
                await _http.DeleteAsync("joke/deleteallcomments");
            }
            catch (Exception ex)
            {

                _ = ex.Message;
            }
        }

        private async Task SetComments(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Comment>>();
            Comments = response;
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
