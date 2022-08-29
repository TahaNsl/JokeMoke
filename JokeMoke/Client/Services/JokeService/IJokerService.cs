namespace JokeMoke.Client.Services.JokeService
{
    public interface IJokerService
    {
        List<Joke> Jokes { get; set; }
        List<JokeType> JokeTypes { get; set; }

        public string Value { get; set; }
        public int JokeTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }

        public string Message { get; set; }

        Task GetJokes();

        Task GetJokeTypes();

        Task CreateJoke(Joke joke);

        Task DeleteJoke(int id);
    }
}
