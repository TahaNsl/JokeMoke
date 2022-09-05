namespace JokeMoke.Client.Services.StatService
{
    public interface IStatService
    {
        public Guid JokeId { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

        Task<JokeStatistics> GetStat(Guid id);

        Task LikeJoke(Guid id, int no);
    }
}
