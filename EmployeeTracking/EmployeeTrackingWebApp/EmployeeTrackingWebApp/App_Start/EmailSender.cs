using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace EmployeeTrackingWebApp
{
    public class EmailSender
    {
        public static void SendEmail(string SendTo, string Subject, string MessageBody)
        {
            string fromEmail = string.Format("{0}@gmail.com", "rvr994");
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(fromEmail);
            mail.To.Add(SendTo);
            mail.Subject = Subject;
            mail.Body = MessageBody;


            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, "rollsroyce18");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

        }
    }
}