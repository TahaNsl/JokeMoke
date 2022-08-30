﻿namespace JokeMoke.Client.Services.JokeService
{
    public interface IJokerService
    {
        List<Joke> Jokes { get; set; }
        List<JokeType> JokeTypes { get; set; }
        List<Comment> Comments { get; set; }

        public string Value { get; set; }
        public int JokeTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }

        public string Message { get; set; }

        Task GetJokes();

        Task GetJokeTypes();

        Task GetComments(int id);

        Task<Joke> GetSingleJoke(int id);

        Task CreateJoke(Joke joke);

        Task CreateComment(Comment comment, int id);

        Task DeleteJoke(int id);
    }
}
