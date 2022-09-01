namespace JokeMoke.Client.Services.UserService
{
    public interface IUserService
    {
        List<User> Users { get; set; }

        List<Role> Roles { get; set; }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePicUrl { get; set; }

        Task GetUsers();

        Task GetRoles();

        Task<User> GetSingleUser(int id);

        Task LoginUser();

        Task LogOutUser();

        Task CreateUser(User user);

        Task DeleteUser(int id);

    }
}
