namespace PVLog.Controllers
{
    public interface IEmailSender
    {
        void Send(string body, string subject, string recipientAddress);
    }
}