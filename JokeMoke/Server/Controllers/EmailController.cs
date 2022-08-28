using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace JokeMoke.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly JokeMokeContext _context;

        private readonly ILogger<EmailController> _logger;

        private readonly IConfiguration _config;

        public EmailController(IConfiguration config, ILogger<JokeMokeContext> logger, JokeMokeContext context)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("Sendmail")]
        public void Sendmail([FromBody] string email)
        {
            try
            {
                var Email = new MimeMessage();
                Email.From.Add(MailboxAddress.Parse("tahanasrollahii@gmail.com"));
                Email.To.Add(MailboxAddress.Parse(email));
                Email.Subject = "Verify Your Email (Taha Blazor)";
                Email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Your email is verified now!</h1>" };

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587); //SecureSocketOptions.StartTls
                smtp.Authenticate("tahanasrollahii@gmail.com", "natson@7869");
                smtp.Send(Email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }

        }
    }
}
