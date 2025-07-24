using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _config;
    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var smtpServer = _config["EmailSettings:SmtpServer"];
        var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"]);
        var fromEmail = _config["EmailSettings:SenderEmail"];
        var fromName = _config["EmailSettings:SenderName"];
        var fromPass = _config["EmailSettings:SenderPassword"];

        var smtpClient = new SmtpClient(smtpServer)
        {
            Port = smtpPort,
            Credentials = new NetworkCredential(fromEmail, fromPass),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(fromEmail, fromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(to);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
