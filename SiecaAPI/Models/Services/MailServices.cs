using System.Net;
using System.Net.Mail;
using SiecaAPI.Commons;

namespace SiecaAPI.Services
{
    public static class MailServices
    {

        public static void SendEmail(string? body, string? subject, string receptor)
        {

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(AppParamsTools.GetEnvironmentVariable("Email:smtpClient"));

            mail.From = new MailAddress(AppParamsTools.GetEnvironmentVariable("Email:MailAddress"));
            mail.To.Add(receptor);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = body;

            SmtpServer.Port = Int32.Parse(AppParamsTools.GetEnvironmentVariable("Email:Port"));
            SmtpServer.Credentials = new System.Net.NetworkCredential(AppParamsTools.GetEnvironmentVariable("Email:User"), AppParamsTools.GetEnvironmentVariable("Email:Password"));
            SmtpServer.EnableSsl = Convert.ToBoolean(AppParamsTools.GetEnvironmentVariable("Email:EnabledSsl"));

            SmtpServer.Send(mail);
        }
    }
}