using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using PlayWithMe.Common.Models;

namespace PlayWithMe.Common.Services
{
    public class EmailService
    {
        public void EmailAvailability(List<SearchItem> items)
        {
            var body = new StringBuilder("<ul>");
            foreach (var item in items)
            {
                body.AppendLine($"<li><a href=\"{item.Url}\">{item.Title}</a></li>");
            }
            body.AppendLine("</ul>");

            var message = new MailMessage();
            message.From = new MailAddress("fruck.bots@gmail.com");
            message.To.Add(new MailAddress("nsearle11@gmail.com"));
            message.Subject = "YOUR BOT - ITEM AVAILABLE!!!!!";
            message.Body = body.ToString();
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587; 
                smtp.Credentials = new NetworkCredential("fruck.bots@gmail.com", "P@ssword123#");

                smtp.Send(message);
            }
        }
    }
}