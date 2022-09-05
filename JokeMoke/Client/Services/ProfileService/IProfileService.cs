
namespace JokeMoke.Client.Services.ProfileService
{
    public interface IProfileService
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePicUrl { get; set; }
        public string Message { get; set; }

        public Task UpdateProfile();
        public Task GetProfile();
    }
}