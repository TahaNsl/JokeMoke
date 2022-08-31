using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace JokeMoke.Client.Services.JokeService
{
    public class JokeService : IJokerService
    {
        public List<Joke> Jokes { get; set; } = new List<Joke>();
        public List<JokeType> JokeTypes { get; set; } = new List<JokeType>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<JokeStatistics> JokeStatisticsList { get; set; } = new List<JokeStatistics>();

        public string Value { get; set; }
        public int JokeTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }

        public string Message { get; set; }


        private HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public JokeService()
        {

        }

        public JokeService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public async Task GetJokes()
        {
            var result = await _http.GetFromJsonAsync<List<Joke>>("joke");
            if (result != null)
            {
                Jokes = result;
            }
        }

        public async Task GetJokeTypes()
        {
            var result = await _http.GetFromJsonAsync<List<JokeType>>("joke/joketypes");
            if (result != null)
            {
                JokeTypes = result;
            }
        }

        public async Task GetComments(int id)
        {
            var result = await _http.GetFromJsonAsync<List<Comment>>($"joke/comments/{id}");
            if (result != null)
            {
                Comments = result;
            }
        }

        public async Task<Joke> GetSingleJoke(int id)
        {
            var result = await _http.GetFromJsonAsync<Joke>($"joke/{id}");
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("Comments Not Found!");
            }
        }

        public async Task<Joke> GetRandomJoke()
        {
            var result = await _http.GetFromJsonAsync<Joke>("joke/Random");
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("هیچ جوکی یافت نشد");
            }
        }

        //public async Task LikeJoke(int id, int which)
        //{
        //    var result = await _http.PostAsJsonAsync("joke/Like");
        //    await SetStats(result);
        //}

        public async Task CreateJoke(Joke joke)
        {
            User currentUser = await _http.GetFromJsonAsync<User>("user/getcurrentuser");

            if(currentUser.Id == 0)
            {
                currentUser = null;
            }

            if(currentUser == null)
            {
                this.Message = "اول وارد شوید";
                _navigationManager.NavigateTo("/login", true);
            }
            else
            {
                joke.CreatedAt = DateTime.UtcNow;
                joke.CreatedBy = currentUser.Id;
                joke.IsApproved = false;
                this.Message = "جوک ساخته شد";

                var result = await _http.PostAsJsonAsync("joke", joke);
                _navigationManager.NavigateTo("/", true);
                //var StatResult = await _http.PostAsJsonAsync("joke/stat", joke.Id);
                //await SetStats(StatResult);
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

        private async Task SetJokes(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Joke>>();
            Jokes = response;
            _navigationManager.NavigateTo("/", true);
        }

        private async Task SetComments(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Comment>>();
            Comments = response;
            _navigationManager.NavigateTo("/", true);
        }

        public async Task DeleteJoke(int id)
        {
            try
            {
                var result = await _http.DeleteAsync($"joke/{id}");
                await SetJokes(result);
            }
            catch (Exception ex)
            {

                _ = ex.Message;
            }

        }

        public static implicit operator JokeService(Joke joke)
        {
            return new JokeService
            {
                Value = joke.Value,
                JokeTypeId = joke.JokeTypeId,
                CreatedAt = joke.CreatedAt,
                CreatedBy = joke.CreatedBy,
                IsApproved = joke.IsApproved
            };
        }

        public static implicit operator Joke(JokeService jokeService)
        {
            return new Joke
            {
                Value = jokeService.Value,
                JokeTypeId = jokeService.JokeTypeId,
                CreatedAt = jokeService.CreatedAt,
                CreatedBy = jokeService.CreatedBy,
                IsApproved = jokeService.IsApproved
            };
        }
    }
}
