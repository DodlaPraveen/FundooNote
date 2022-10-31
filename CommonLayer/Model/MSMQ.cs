using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace CommonLayer.Model
{
   
    public class MSMQ
    {
        MessageQueue message = new MessageQueue();
        public void sendData2Queue(string token)
        {
            message.Path = @".\private$\Token";
            if (!MessageQueue.Exists(message.Path))
            {
                MessageQueue.Create(message.Path);
            }
            
                message.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                message.ReceiveCompleted += Message_ReceiveCompleted;
                message.Send(token);
                message.BeginReceive();
                message.Close();
                // Creates the new queue named "Bills"
                
          

        }

        private void Message_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var msg = message.EndReceive(e.AsyncResult);
                string token = msg.Body.ToString();
                string Subject = "Forget Password Token";
                string Body = $"Forget Your Password?\nTo Reset Your password copy this token and paste it\n\n" + token;
                string jwt = jwtToken(token);
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("dodlabl09@gmail.com", "fucrrvybrtfrfwki"),//give dummy gmail
                    EnableSsl = true,
                };

                smtpClient.Send("dodlabl09@gmail.com", "dodlabl09@gmail.com",  Subject, Body);
                //SMTP.Send("shravanthi27041996@gmail.com", "shravanthi27041996@gmail.com", Subject, Body);
                message.BeginReceive();
            }
            catch (Exception)
            {

                throw;
            }

        }
        public string jwtToken(string token)
        {
            var decodedToken = token;
            var handlaer = new JwtSecurityTokenHandler();
            var jsonToken = handlaer.ReadJwtToken((decodedToken));
            var result = jsonToken.Claims.FirstOrDefault().Value;
            return result;
        }
    }
}
