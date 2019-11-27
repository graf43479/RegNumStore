using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
   public class DeliveryProcessor : IDeliveryProcessor
    {

       private EmailSettings emailSettings;
       //private EmailSettings operatorEmailSettings;
         private RegNumDBContext context;


         public DeliveryProcessor(RegNumDBContext context, EmailSettings settings, EmailSettings operatorSettings)
        {
            this.context = context;
            emailSettings = settings;
           // operatorEmailSettings = operatorSettings;
            
            
          
        }


       public void EmailRecovery(User user, string host)
       {
           EmailTest();
           using (var smtpClient = new SmtpClient())
           {
           //    emailSettings.MailToAddress = user.Email;
               smtpClient.EnableSsl = emailSettings.UseSsl;
               smtpClient.Host = emailSettings.ServerName;
               smtpClient.Port = emailSettings.ServerPort;
               smtpClient.UseDefaultCredentials = false;
               smtpClient.Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password);

               if (emailSettings.WriteAsFile)
               {
                   smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                   smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                   smtpClient.EnableSsl = false;
               }

               StringBuilder body = new StringBuilder()
                   .AppendLine("<p>Здравствуйте " + user.Login + "! Ваш логин и пароль для авторизации на сайте: </p>")
                   .AppendLine("<p>------------------------------------</p>")
                   .AppendLine("<p>Логин: " + user.Login + "</p>")
                   .AppendLine("<p>Пароль: " + user.Password + "</p>")
                   .AppendLine("<p>------------------------------------</p>")
                   .AppendLine("<p>Для авторизации пройдите <a href='http://" + host + "/Account/Login'>по ссылке</a></p>");

               MailMessage mailMessage = new MailMessage(
                   emailSettings.MailFromAddress,
               //    emailSettings.MailToAddress,
               user.Email,
                   "Восстановление пароля",
                   body.ToString()
                   );




               mailMessage.IsBodyHtml = true;

               if (emailSettings.WriteAsFile)
               {
                   mailMessage.BodyEncoding = Encoding.UTF8;
               }


               try
               {
                   smtpClient.Send(mailMessage);
               }
               catch (Exception ex)
               {
                   Console.WriteLine(ex);
               }
           }
       }

       public void EmailActivation(User user, string host)
       {
           EmailTest();
           string activationLink = "<p>Добрый день, " + user.Login + "</p><p>Спасибо за интерес, проявленный к нашему сайту</p>" +
               "<p>Вы получили уведомление, потому что была произведена регистрация Вашего адреса</p>" +
               "<p>Для активизации Вашего аккаунта пройдите по ниже следующей ссылке</p>";
           //   activationLink= activationLink + "<p> <a href='http://localhost:57600/Account/Activate/" + user.Login + "/" + user.NewEmailKey + "'> http://localhost:57600/Account/Activate/" + user.Login + "/" + user.NewEmailKey + "</a></p>";
           activationLink = activationLink + "<p> <a href='http://" + host + "/Account/Activate/" + user.Login + "/" + user.NewEmailKey + "'> http://" + host + "/Account/Activate/" + user.Login + "/" + user.NewEmailKey + "</a></p>";
           activationLink = activationLink + "<p>Ваш логин: " + user.Login + "</p>" + "<p>Ваш пароль: " + user.Password + "</p>" +
           "<p>Если Вы не предпринимали попытку регистрации на сайте, то, пожалуйста, проигнорируйте данное письмо</p>";

           //string url = HttpWebRequest.
           /* MailMessage mailMessage = new MailMessage(
               emailSettings.MailFromAddress,
               emailSettings.MailToAddress,
               "Активация аккаунта",
               activationLink
               );
            
            mailMessage.IsBodyHtml = true;*/

           MailMessage mailMessage = new MailMessage(
                       emailSettings.MailFromAddress,
               //emailSettings.MailToAddress,
                       user.Email,
                       "Активация аккаунта",
                       activationLink
                       );
      

           Mailer(mailMessage);
       }

       public async Task Mailer(MailMessage mailMessage)
       {

           mailMessage.IsBodyHtml = true;


           if (emailSettings.WriteAsFile)
           {
               mailMessage.BodyEncoding = Encoding.UTF8;
           }

           using (var smtpClient = new SmtpClient())
           {
               // emailSettings.MailToAddress = user.Email;
               smtpClient.EnableSsl = emailSettings.UseSsl;
               smtpClient.Host = emailSettings.ServerName;
               smtpClient.Port = emailSettings.ServerPort;
               smtpClient.UseDefaultCredentials = false;
               smtpClient.Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password);
               try
               {
                   smtpClient.Send(mailMessage);
               }
               catch (Exception)
               {

               }
           }
       }

       public void FeedBackRequest(Message message)
       {
           EmailTest();
           string activationLink = "<p>Добрый день! Клиент сайта <b>" + Constants.SITE_NAME + "</b>, представившийся " +
                                   "как <b>" + message.Name + "</b>, направил вам сообщение с обратным адресом: " +
                                   message.Email + "</p>" +
                                   "<p>Текст сообщения ниже</p>" +
                                   "<p style='font-weight: bold;color: indigo;background-color: lavender'>" +
                                   message.Text + "</p>" +
                                   "<p>Отвечайте на письмо на адрес: " + message.Email + "</p>"; 

           //string url = HttpWebRequest.
           /* MailMessage mailMessage = new MailMessage(
               emailSettings.MailFromAddress,
               emailSettings.MailToAddress,
               "Активация аккаунта",
               activationLink
               );
            
            mailMessage.IsBodyHtml = true;*/

           MailMessage mailMessage = new MailMessage(
                       emailSettings.MailFromAddress,
               emailSettings.MailFromAddress,
               //        user.Email,
                       Constants.SITE_NAME + ": " + message.Name + " написал сообщение",
                       activationLink
                       );
           
           Mailer(mailMessage);

           activationLink = "<p>Добрый день, <b>" + message.Name + "</b>! На почту сайта <b>" + Constants.SITE_NAME + "</b> было направлено письмо! " +
                                   "Наши специалисты ответят Вам при первой же возможности." +
                                   "<p>Текст Вашего сообщения:</p>" +
                                   "<p style='font-weight: bold;color: indigo;background-color: lavender'>" +
                                   message.Text + "</p>";
           MailMessage mailMessageToUser = new MailMessage(
               emailSettings.MailFromAddress,
               message.Email,
               //        user.Email,
                        "NoReply: Письмо на " + Constants.SITE_NAME,
                       activationLink
                       );
           Mailer(mailMessageToUser);
         //  mailMessage.Body = "<p>Добрый день, <b>" + message.Name + "</b>! На почту сайта <b>" + Constants.SITE_NAME + "</b> было направлено письмо! " +
         //                          "Наши специалисты ответят Вам при первой же возможности." +
         //                          "<p>Текст Вашего сообщения:</p>" +
         //                          "<p style='font-weight: bold;color: indigo;background-color: lavender'>" +
         //                          message.Text + "</p>";
         ////  mailMessage.To.Clear();
         //  mailMessage.To.Add(emailSettings.MailToAddress);
         //  mailMessage.Subject = "NoReply: Письмо на " + Constants.SITE_NAME;
         //  Mailer(mailMessage);

       }

       public void OrderQuickSale(Order order, string orderOrQuickSale)
       {
           EmailTest();
           
           Message message = new Message
           {
               Name = order.Name,
               Email = order.Email,
               Text = order.Comment

           };

           string activationLink = "";
           string operatorMessage = "";
           if (orderOrQuickSale == "quickSale")
           {
               activationLink = "<p>Добрый день, <b>" + order.Name + "</b>! На сайте <b>" + Constants.SITE_NAME +
                                "</b> был размещен заказ на срочную продажу автомобильного номера" +
                                "<b>" + order.ProductNumber +
                                "</b>. Наши специалисты сделают всё возможное, чтобы найти на него покупателя в кратчайшие сроки. ";

               operatorMessage = "<p>Добрый день! Клиент сайта <b>" + Constants.SITE_NAME + "</b>, представившийся " +
                                   "как <b>" + message.Name + "</b>, разместил заказ на срочную продажу номера <b>" + order.ProductNumber +
                                "</b>Обратный адрес: " +
                                   message.Email + "</p><p>Отвечайте на письмо на адрес: " + message.Email + "</p>" +
                                 "<p>Телефон: <b>"+ order.Phone +"</b></p>"; 

               if (!String.IsNullOrEmpty(order.Comment))
               {
                   activationLink += "<p>Ваш комменатрий к заказу:</p>" +
                                  "<p style='font-weight: bold;color: indigo;background-color: lavender'>" +
                                  message.Text + "</p>" ;

                   operatorMessage += "<p>Комменатрий к заказу:</p>" +
                                  "<p style='font-weight: bold;color: indigo;background-color: lavender'>" +
                                  message.Text + "</p>";
               }
           }
           else
           {
               activationLink = "<p>Добрый день, <b>" + order.Name + "</b>! На сайте <b>" + Constants.SITE_NAME +
                                 "</b> был размещен заказ на поиск автомобильного номера" +
                                 "<b>" + order.ProductNumber +
                                 "</b>. Наши специалисты сделают всё возможное, чтобы найти его продавца в кратчайшие сроки. ";

               operatorMessage = "<p>Добрый день! Клиент сайта <b>" + Constants.SITE_NAME + "</b>, представившийся " +
                               "как <b>" + message.Name + "</b>, разместил заказ на поиск номера <b>" + order.ProductNumber +
                            "</b>. Обратный адрес: " +
                               message.Email + "</p><p>Отвечайте на письмо на адрес: " + message.Email + "</p>" +
               "<p>Телефон: <b>" + order.Phone + "</b></p>"; 


               if (!String.IsNullOrEmpty(order.Comment))
               {
                   activationLink += "<p>Ваш комменатрий к заказу:</p>" +
                                  "<p style='font-weight: bold;color: indigo;background-color: lavender'>" +
                                  message.Text + "</p>";

                     operatorMessage += "<p>Комменатрий к заказу:</p>" +
                                  "<p style='font-weight: bold;color: indigo;background-color: lavender'>" +
                                  message.Text + "</p>";
               }
           }


           
           //string url = HttpWebRequest.
           /* MailMessage mailMessage = new MailMessage(
               emailSettings.MailFromAddress,
               emailSettings.MailToAddress,
               "Активация аккаунта",
               activationLink
               );
            
            mailMessage.IsBodyHtml = true;*/

           MailMessage mailMessage = new MailMessage(
                       emailSettings.MailFromAddress,
               emailSettings.MailFromAddress,
               //        user.Email,
                       Constants.SITE_NAME + ": " + message.Name + " оставил заказ",
                       operatorMessage
                       );

           Mailer(mailMessage);

          
           MailMessage mailMessageToUser = new MailMessage(
               emailSettings.MailFromAddress,
               message.Email,
               //        user.Email,
                        "NoReply: Письмо на " + Constants.SITE_NAME,
                       activationLink
                       );
           Mailer(mailMessageToUser);

         
       }


       public void EmailTest()
       {
           var sets = context.MailSettingses.AsNoTracking().ToList();

           try
           {
               emailSettings.MailFromAddress = sets.FirstOrDefault(x => x.MailSettingsID == "MAIL_FROM_ADDRESS").SettingsValue;
               emailSettings.UseSsl = Boolean.Parse(sets.FirstOrDefault(x => x.MailSettingsID == "MAIL_USE_SSL").SettingsValue);
               emailSettings.UserName = sets.FirstOrDefault(x => x.MailSettingsID == "MAIL_SERVER_USER_NAME").SettingsValue;
               emailSettings.Password = sets.FirstOrDefault(x => x.MailSettingsID == "MAIL_SERVER_PASSWORD").SettingsValue;
               emailSettings.ServerName = sets.FirstOrDefault(x => x.MailSettingsID == "MAIL_SERVER_NAME").SettingsValue;
               emailSettings.ServerPort = Int32.Parse(sets.FirstOrDefault(x => x.MailSettingsID == "MAIL_SERVER_PORT").SettingsValue);
               emailSettings.WriteAsFile = Boolean.Parse(sets.FirstOrDefault(x => x.MailSettingsID == "MAIL_WRITE_AS_FILE").SettingsValue);
               emailSettings.FileLocation =
                   sets.FirstOrDefault(x => x.MailSettingsID == "MAIL_FILE_LOCATION").SettingsValue;


           }
           catch (Exception)
           {
               
               throw;
           }
           //emailSettings.MailFromAddress = ((sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_FROM_ADDRESS"))!=null) ? sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_FROM_ADDRESS").SettingsValue : Constants.MAIL_FROM_ADDRESS;
           //emailSettings.UseSsl = ((sets.FirstOrDefault(x => x.MailSettingsID == "MAIL_USE_SSL")) != null) ? Boolean.Parse(sets.FirstOrDefault(x => x.MailSettingsID == "MAIL_USE_SSL").SettingsValue) : Constants.USE_SSL;
           //emailSettings.UserName = ((sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_SERVER_USER_NAME"))!=null) ? sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_FROM_ADDRESS").SettingsValue : Constants.USERNAME;
           //emailSettings.Password = ((sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_SERVER_PASSWORD"))!=null) ? sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_FROM_ADDRESS").SettingsValue : Constants.PASSWORD;
           //emailSettings.ServerName = ((sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_SERVER_NAME"))!=null) ? sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_FROM_ADDRESS").SettingsValue : Constants.SERVERNAME;
           //emailSettings.ServerPort = ((sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_SERVER_PORT"))!=null) ? Int32.Parse(sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_FROM_ADDRESS").SettingsValue) : Constants.SERVER_PORT;
           //emailSettings.WriteAsFile = ((sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_WRITE_AS_FILE"))!=null) ? Boolean.Parse(sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_FROM_ADDRESS").SettingsValue) : Constants.WRITE_AS_FILE;
           //emailSettings.FileLocation = ((sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_FILE_LOCATION"))!=null) ? sets.FirstOrDefault(x=>x.MailSettingsID=="MAIL_FROM_ADDRESS").SettingsValue : @"c:/sportstore";//Constants.FILE_LOCATION;
           
   
                  
           
           //emailSettings.MailFromAddress = Constants.MAIL_FROM_ADDRESS;
               //emailSettings.UseSsl = Constants.USE_SSL;
               //emailSettings.UserName = Constants.USERNAME;
               //emailSettings.Password = Constants.PASSWORD;
               //emailSettings.ServerName = Constants.SERVERNAME;
               //emailSettings.ServerPort = Constants.SERVER_PORT;
               //emailSettings.WriteAsFile = Constants.WRITE_AS_FILE;
               //emailSettings.FileLocation = @"c:/sportstore";//Constants.FILE_LOCATION;
           

        
               //operatorEmailSettings.MailFromAddress = Constants.MAIL_FROM_ADDRESS;
               //operatorEmailSettings.UseSsl = Constants.USE_SSL;
               //operatorEmailSettings.UserName = Constants.USERNAME;
               //operatorEmailSettings.Password = Constants.PASSWORD;
               //operatorEmailSettings.ServerName = Constants.SERVERNAME;
               //operatorEmailSettings.ServerPort = Constants.SERVER_PORT;
               //operatorEmailSettings.WriteAsFile = Constants.WRITE_AS_FILE;
               //operatorEmailSettings.FileLocation = @"c:/sportstore";
        
       }


       public class EmailSettings
       {
           public string MailToAddress;
           public string MailFromAddress;
           public bool UseSsl;
           public string UserName;
           public string Password;
           public string ServerName;
           public int ServerPort;
           public bool WriteAsFile;
           public string FileLocation;
       }
    }
}
