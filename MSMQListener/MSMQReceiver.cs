using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace MSMQListener
{
    public class MSMQReceiver
    {
        public void ReceiveMessageFromQueue(string email, string token)
        {
            try
            {
                string url = "http://localhost:4200/reset" +
                    "/" + token;

                MailMessage mail = new MailMessage();

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                mail.From = new MailAddress("sandhyapatil364@gmail.com");
                mail.To.Add(new MailAddress(email));
                mail.Subject = "Link for Reseting Password";
                mail.Body = "Click on link " + url + " to reset the password  Token: ";

                smtpClient.Credentials = new System.Net.NetworkCredential("sandhyapatil364@gmail.com", "Sandhya@1995");
                smtpClient.EnableSsl = true;

                smtpClient.Send(mail);
                Console.WriteLine("link has been sent to your mail....");
            }
            catch (Exception exception)
            {
                Console.Write(exception.ToString());
            }
            Console.WriteLine("Message received ......");
        }
    }
}
