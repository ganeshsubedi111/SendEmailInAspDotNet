using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Newtonsoft.Json;
using System;
namespace SendEmail.Controllers
{
    public class SendEmailController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmails(string senderEmailAddress, string jsonResponse, string subject)
        {
            try
            {

                var emailTemplate = ParseJsonToEmailTemplate(jsonResponse);


                SendEmail(senderEmailAddress, emailTemplate, subject);

                return Json(new { success = true, message = "Email sent successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string ParseJsonToEmailTemplate(string jsonResponse)
        {

            dynamic jsonObj = JsonConvert.DeserializeObject(jsonResponse);
            return jsonObj.htmlContent;
        }


        private void SendEmail(string toEmailAddress, string emailBody, string subject)
        {
            var fromEmail = "example@gmail.com";
            var fromPassword = "Your -password for  example@gmail.com in encrypted for"; // for this get password from google account security  generate password and add do not add original passowrd
            var smtpHost = "smtp.gmail.com";
            var smtpPort = 587;

            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = emailBody,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmailAddress);


            smtpClient.Send(mailMessage);
        }
    }
    }
