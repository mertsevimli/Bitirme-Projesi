using Sarideniz.Core.Entities;
using System.Net;
using System.Net.Mail;

namespace Sarideniz.WebUI.Utils;

public class MailHelper
{
    public static async Task <bool> SendEmailAsync(Contact contact)
    {
        SmtpClient smtpClient = new SmtpClient("mail.sarideniz.com", 587);
        smtpClient.Credentials = new NetworkCredential("info@sarideniz.com", "sarideniz");
        smtpClient.EnableSsl = false;
        MailMessage message = new MailMessage();
        message.From = new MailAddress("info@sarideniz.com", "Sarideniz");
        message.To.Add("bilgi@sarideniz.com");
        message.Subject = "Sarideniz'den Mesajınız Var!";
        message.Body = $"İsim : {contact.Name} - Soyisim : {contact.Surname} - Email : {contact.Email} - Telefon : {contact.Phone} - Mesaj : {contact.Message} ";
        message.IsBodyHtml = true;
        try
        {
            await smtpClient.SendMailAsync(message);
            smtpClient.Dispose();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
        
    }
    public static async Task <bool> SendEmailAsync(string email,string subject ,string mailBody )
    {
        SmtpClient smtpClient = new SmtpClient("mail.sarideniz.com", 587);
        smtpClient.Credentials = new NetworkCredential("info@sarideniz.com", "sarideniz");
        smtpClient.EnableSsl = false;
        MailMessage message = new MailMessage();
        message.From = new MailAddress("info@sarideniz.com", "Sarideniz");
        message.To.Add(email);
        message.Subject = subject;
        message.Body = mailBody;
        message.IsBodyHtml = true;
        try
        {
            await smtpClient.SendMailAsync(message);
            smtpClient.Dispose();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
        
    }
}