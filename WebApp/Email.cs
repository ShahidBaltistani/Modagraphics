using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

namespace WebApp
{
    public class Email
    {
        public string FromSubject { get; set; } = "Modagrafics - ";
        public static string FromMail { get; set; }
        public static string FromPassword { get; set; }
        public static string Host { get; set; }
        public static int Port { get; set; }
        public Email()
        {
            if (string.IsNullOrEmpty(FromMail) || string.IsNullOrEmpty(FromPassword) || string.IsNullOrEmpty(Host))
            {
                var configrations = EmailConfigurationSetup.GetConfigration();
                if (configrations != null)
                {
                    FromMail = configrations.Email;
                    FromPassword = configrations.Password;
                    Host = configrations.Host;
                    Port = configrations.Port;
                }
            }


        }
        public async Task<bool> ForgotPassword(LoginModel model)
        {
            try
            {
                var message = new MailMessage();
                message.To.Add(new MailAddress(model.Email));
                message.From = new MailAddress(FromMail); message.Subject = FromSubject +" Forget Password";
                message.IsBodyHtml = true;
                message.AlternateViews.Add(forgotPassword_Body(model));

                using (var smtp = new System.Net.Mail.SmtpClient())
                {
                    smtp.UseDefaultCredentials = true;
                    var credential = new NetworkCredential
                    {
                        UserName = FromMail,
                        Password = FromPassword
                    };
                    smtp.Credentials = credential;
                    smtp.Host = Host;
                    smtp.Port = Port;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        private AlternateView forgotPassword_Body(LoginModel model)
        {
            string str = @"
        <section id='content'>
            <div class='container center clearfix>
            <div class='heading-block center'>
                 <h2>Hi " + model.Name + @",</h2>
                <div>We're sending you this email because you requested for password forget.Your account password is given below.  
                </div>
                <div>
                 <p>Password:" + model.Password + @"</p>
                </div>

             <div><br/>Best Regards,<br/>
                Modagpraphics Team
             </div>
            </div>
            </div>
            </section>";
            AlternateView AV =
            AlternateView.CreateAlternateViewFromString(str, null, MediaTypeNames.Text.Html);

            return AV;
        }

        private async Task<bool> sendEmail(MailMessage message)
        {
            try
            {
                using (var smtp = new System.Net.Mail.SmtpClient())
                {
                    smtp.UseDefaultCredentials = true;
                    var credential = new NetworkCredential
                    {
                        UserName = FromMail,
                        Password = FromPassword
                    };
                    smtp.Credentials = credential;
                    smtp.Host = Host;
                    smtp.Port = Port;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}