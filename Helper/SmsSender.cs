using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using BackofficeDemo.Common.Helpers;
using Plivo.API;

namespace Helper
{
    public class SmsSender
    {
        public static string SendSms(string destPhone, string text)
        {
            var sendtoemail = ConfigurationManager.AppSettings["Sms:SendToEmail"].ToLower() == "true";
            var email = ConfigurationManager.AppSettings["Sms:EmailForSms"];
            if (!sendtoemail)
            {
                var plivo = new RestAPI(SettingsManager.PlivoAuthId, SettingsManager.PlivoAuthToken);

                var resp = plivo.send_message(new Dictionary<string, string>()
                {
                    {"src", SettingsManager.PlivoFromPhone},
                    {"dst", destPhone},
                    {"text", text},
                    {"method", "POST"}
                });

                if (!string.IsNullOrEmpty(resp.Data.error))
                {
                    throw new Exception(resp.Data.error);
                }

                return resp.Data.message_uuid[0];
            }
           

            try
            {
                var message = new EmailMessage();
                message.Subject = "Thanks for Your Order";
                message.Body = $"{destPhone}: {text}";
                message.From = SettingsManager.EmailToCustomerFrom;
                message.To = email;

                EmailSender.SendAsyncEmail(message);
            }
            catch
            {
                    
            }
            return "toemail";
        }

        public static async Task SendSmsAsync(string destPhone, string text)
        {
            await Task.Run(() =>
            {
                //    var error1 = new Error()
                //{
                //    HostName = "Try sms send",
                //        User = "User"
                //    };

                //ErrorLog.Default.Log(error1);

                SendSms(destPhone, text);

                try
                {
                    SendSms(destPhone, text);
                }
                catch (Exception ex)
                {
                    var message = $"DestPhone: {destPhone}, Text: {text}. Exception: {ex}";

                    EmailSender.SendEmailToAdmins("error - exception while sending sms to client", message);
                }
            });   
        }
    }
}
    