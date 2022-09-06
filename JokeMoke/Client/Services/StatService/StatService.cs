using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace JokeMoke.Client.Services.StatService
{
    public class StatService : IStatService
    {
        public Guid JokeId { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

        public List<JokeStatistics> JokeStatisticsList { get; set; } = new List<JokeStatistics>();

        private HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public StatService()
        {

        }

        public StatService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public async Task<JokeStatistics> GetStat(Guid id)
        {
            var result = await _http.GetFromJsonAsync<JokeStatistics>($"joke/stat/{id}");

            return result;
        }

        public async Task LikeJoke(Guid id, int no)
        {
            User currentUser = await _http.GetFromJsonAsync<User>("user/getcurrentuser");

            if (currentUser.Id == Guid.Empty)
            {
                currentUser = null;
            }

            if (currentUser == null)
            {
                _navigationManager.NavigateTo("/login", true);
            }
            else
            {
                await _http.PutAsJsonAsync($"joke/like/{id}/{no}", currentUser);
            }
        }

        private async Task SetStats(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<JokeStatistics>>();
            JokeStatisticsList = response;
            _navigationManager.NavigateTo("/", true);
        }

        public static implicit operator StatService(JokeStatistics jokeStatistics)
        {
            return new StatService
            {
                JokeId = jokeStatistics.JokeId,
                LikeCount = jokeStatistics.LikeCount,
                DislikeCount = jokeStatistics.DislikeCount
            };
        }

        public static implicit operator JokeStatistics(StatService statService)
        {
            return new JokeStatistics
            {
                JokeId = statService.JokeId,
                LikeCount = statService.LikeCount,
                DislikeCount = statService.DislikeCount
            };
        }
    }
}
