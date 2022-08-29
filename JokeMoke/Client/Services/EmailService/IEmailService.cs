namespace JokeMoke.Client.Services.EmailService
{
    public interface IEmailService
    {
        public Task Sendmail(string email);
    }
}
