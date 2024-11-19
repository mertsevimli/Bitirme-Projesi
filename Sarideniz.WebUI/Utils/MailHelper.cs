using Sarideniz.Core.Entities;
using System.Net;
using System.Net.Mail;

namespace Sarideniz.WebUI.Utils;

public class MailHelper
{
    public static async Task SendEmailAsync(Contact contact)
    {
        SmtpClient smtpClient = new SmtpClient("mail.sarideniz.com", 587);
        smtpClient.Credentials = new NetworkCredential("info@sarideniz.com", "sarideniz");
        smtpClient.EnableSsl = false;
        MailMessage message = new MailMessage();
        message.From = new MailAddress("info@sarideniz.com", "Sarideniz");
        message.To.Add(contact.Email);
        message.Subject = "Sarideniz'den Mesajınız Var!";
        message.Body = $"İsim : {contact.Name} - Soyisim : {contact.Surname} - Email : {contact.Email} - Telefon : {contact.Phone} - Mesaj : {contact.Message} ";
        message.IsBodyHtml = true;
        await smtpClient.SendMailAsync(message);
        smtpClient.Dispose();
    }
}