using System;
using System.Threading;

namespace BackofficeDemo.Common.Helpers
{
    public class EmailSender
    {
        public static void SendAsyncEmail(EmailMessage message)
        {
            SendAsyncEmail(message.From, message.To, message.Subject, message.Body);
        }

        public static void SendAsyncEmail(string from, string to, string subject, string body)
        {
            if (!String.IsNullOrEmpty(from) && !String.IsNullOrEmpty(to) && !String.IsNullOrEmpty(subject) && !String.IsNullOrEmpty(body))
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    try
                    {
                        using (var smtp = new System.Net.Mail.SmtpClient())
                        {
                            using (var mail = new System.Net.Mail.MailMessage())
                            {
                                mail.To.Add(to);
                                mail.Subject = subject;
                                mail.From = new System.Net.Mail.MailAddress(from);
                                mail.IsBodyHtml = true;
                                mail.Body = body;

                                smtp.Send(mail);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var message = String.Format("From: {0}, To: {1}, Subject: {2}, Body: {3}. Details: {4}", from, to, subject, body, ex.ToString());

                        SendEmailToAdmins("error - exception while sending async email to client", message);
                    }
                });
            }
            else
            {
                var message = String.Format("From: '{0}', To: '{1}', Subject: '{2}' or Body: '{3}' is empty!");

                SendEmailToAdmins("error - missing parameters", message);
            }
        }

        public static void SendEmailToAdmins(string subject, string body)
        {
            System.Net.NetworkCredential cred = new System.Net.NetworkCredential("from@gmail.com", "passss");

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add("youremail@gmail.com");
            mail.Subject = subject;
            mail.From = new System.Net.Mail.MailAddress("from@gmail.com");
            mail.IsBodyHtml = true;
            mail.Body = body;

            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = cred;
            smtp.Port = 587;
            smtp.Send(mail);
        }
    }

    public class EmailMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public override string ToString()
        {
            var template = "Email was sent.<br /><br />From: '{0}'<br />, To: '{1}'<br />, Subject: '{2}'<br />, Body<br /><iframe style='width: 100%; border: 1px solid grey;'>{3}</iframe>";

            return String.Format(template, this.From, this.To, this.Subject, this.Body);
        }
    }
}
