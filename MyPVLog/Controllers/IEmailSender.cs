namespace PVLog.Controllers
{
    using System.Net.Mail;

    public interface IEmailSender
    {
        void Send(string body, string subject, string recipientAddress);
    }

    internal class EmailSender : IEmailSender
    {
        public void Send(string body, string subject, string recipientAddress)
        {
            new SmtpClient("smtp.server.com", 25).Send("info@mypvlog.de",
                recipientAddress,
                subject,
                body);
        }
    }
}