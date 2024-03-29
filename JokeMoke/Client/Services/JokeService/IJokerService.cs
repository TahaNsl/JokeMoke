﻿namespace JokeMoke.Client.Services.JokeService
{
    public interface IJokerService
    {
        List<Joke> Jokes { get; set; }
        List<Joke> MyJokes { get; set; }
        List<Joke> ApprovedJokes { get; set; }
        List<Joke> NotApprovedJokes { get; set; }
        List<JokeType> JokeTypes { get; set; }
        List<Comment> Comments { get; set; }
        List<JokeStatistics> JokeStatisticsList { get; set; }

        public string Value { get; set; }
        public Guid JokeTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsApproved { get; set; }

        public string Message { get; set; }

        Task GetJokes();

        Task GetApprovedJokes();

        Task GetNotApprovedJokes();

        Task GetJokeTypes();

        Task<Joke> GetSingleJoke(Guid id);

        Task<Joke> GetRandomJoke();

        Task GetMyJokes();

        Task CreateJoke(Joke joke);

        Task DeleteJoke(Guid id);

        Task DeleteAllJokes();

        Task ApproveJoke(Guid id);
    }
}
