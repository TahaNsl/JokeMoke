namespace JokeMoke.Client.Services.StatService
{
    public interface IStatService
    {
        public int JokeId { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }

        Task<JokeStatistics> GetStat(int id);

        Task LikeJoke(int id, int no);
    }
}
