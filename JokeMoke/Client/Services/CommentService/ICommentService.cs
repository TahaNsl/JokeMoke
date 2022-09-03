namespace JokeMoke.Client.Services.CommentService
{
    public interface ICommentService
    {
        List<Joke> Jokes { get; set; }
        List<JokeType> JokeTypes { get; set; }
        List<Comment> Comments { get; set; }
        List<Comment> AllComments { get; set; }
        List<Comment> ApprovedComments { get; set; }
        List<Comment> NotApprovedComments { get; set; }

        public int JokeId { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsApproved { get; set; }

        public string Message { get; set; }

        Task GetAllComments();

        Task GetComments(int id);

        Task GetApprovedComments();

        Task GetNotApprovedComments();

        Task CreateComment(Comment comment, int id);

        Task DeleteComment(int id);

        Task ApproveComment(int id);

        Task DeleteAllComments();
    }
}
