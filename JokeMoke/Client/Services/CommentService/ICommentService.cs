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

        public Guid JokeId { get; set; }
        public string Value { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public bool IsApproved { get; set; }

        public string Message { get; set; }

        Task GetAllComments();

        Task GetComments(Guid id);

        Task GetApprovedComments();

        Task GetNotApprovedComments();

        Task CreateComment(Comment comment, Guid id);

        Task DeleteComment(Guid id);

        Task ApproveComment(Guid id);

        Task DeleteAllComments();
    }
}
