namespace JokeMoke.Client.Services.CommentService
{
    public interface ICommentService
    {
        List<Joke> Jokes { get; set; }
        List<JokeType> JokeTypes { get; set; }
        List<Comment> Comments { get; set; }

        public int JokeId { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }

        public string Message { get; set; }

        Task GetComments(int id);

        Task CreateComment(Comment comment, int id);
    }
}
