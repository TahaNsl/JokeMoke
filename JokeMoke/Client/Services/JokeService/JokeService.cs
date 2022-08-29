﻿using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace JokeMoke.Client.Services.JokeService
{
    public class JokeService : IJokerService
    {
        public List<Joke> Jokes { get; set; } = new List<Joke>();
        public List<JokeType> JokeTypes { get; set; } = new List<JokeType>();

        public string Value { get; set; }
        public int JokeTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }

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

        public async Task CreateJoke(Joke joke)
        {
            var result = await _http.PostAsJsonAsync("joke", joke);
            await SetJokes(result);
        }

        private async Task SetJokes(HttpResponseMessage result)
        {
            var response = await result.Content.ReadFromJsonAsync<List<Joke>>();
            Jokes = response;
            _navigationManager.NavigateTo("/index", true);
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
    }
}