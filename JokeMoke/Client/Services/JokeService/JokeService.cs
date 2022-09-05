using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace JokeMoke.Client.Services.JokeService
{
    public class JokeService : IJokerService
    {
        public List<Joke> Jokes { get; set; } = new List<Joke>();
        public List<Joke> MyJokes { get; set; } = new List<Joke>();
        public List<Joke> ApprovedJokes { get; set; } = new List<Joke>();
        public List<Joke> NotApprovedJokes { get; set; } = new List<Joke>();
        public List<JokeType> JokeTypes { get; set; } = new List<JokeType>();
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public List<JokeStatistics> JokeStatisticsList { get; set; } = new List<JokeStatistics>();

        public string Value { get; set; }
        public Guid JokeTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
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
            try
            {
                var result = await _http.GetFromJsonAsync<List<Joke>>("joke");
                if (result != null && result.Count() != 0)
                {
                    Jokes = result;
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

        public async Task GetApprovedJokes()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<Joke>>("joke/getapprovedjokes");
                if (result != null && result.Count() != 0)
                {
                    ApprovedJokes = result;
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

        public async Task GetNotApprovedJokes()
        {
            try
            {
                var result = await _http.GetFromJsonAsync<List<Joke>>("joke/getnotapprovedjokes");
                if (result != null && result.Count() != 0)
                {
                    NotApprovedJokes = result;
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

        public async Task GetJokeTypes()
        {
            var result = await _http.GetFromJsonAsync<List<JokeType>>("joke/joketypes");
            if (result != null)
            {
                JokeTypes = result;
            }
        }

        public async Task<Joke> GetSingleJoke(Guid id)
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
            var result = await _http.GetFromJsonAsync<Joke>("joke/random");
            if (result != null)
            {
                return result;
            }
            else
            {
                throw new Exception("هیچ جوکی یافت نشد");
            }
        }

        public async Task GetMyJokes()
        {
            User currentUser = await _http.GetFromJsonAsync<User>("user/getcurrentuser");
            Guid id = currentUser.Id;
            try
            {
                var result = await _http.GetFromJsonAsync<List<Joke>>($"joke/MyJokes/{id}");
                if (result != null)
                {
                    MyJokes = result;
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
        }

        public async Task CreateJoke(Joke joke)
        {
            User currentUser = await _http.GetFromJsonAsync<User>("user/getcurrentuser");

            if(currentUser.Id == Guid.Empty)
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
                _navigationManager.NavigateTo("/myjokes", true);
                //var StatResult = await _http.PostAsJsonAsync("joke/stat", joke.Id);
                //await SetStats(StatResult);
            }
            
        }

        public async Task DeleteJoke(Guid id)
        {
            try
            {
                await _http.DeleteAsync($"joke/{id}");
            }
            catch (Exception ex)
            {

                _ = ex.Message;
            }
        }

        public async Task DeleteAllJokes()
        {
            try
            {
                await _http.DeleteAsync("joke/deletealljokes");
            }
            catch (Exception ex)
            {

                _ = ex.Message;
            }
        }

        public async Task ApproveJoke(Guid id)
        {
            Joke joke = new Joke();

            await _http.PutAsJsonAsync($"joke/approvejoke/{id}", joke);
        }

        private async Task SetJokes(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Joke>>();
            Jokes = response;
            _navigationManager.NavigateTo("/", true);
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
