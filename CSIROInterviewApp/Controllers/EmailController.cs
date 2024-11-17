using CSIROInterviewApp.Models;
using CSIROInterviewApp.Models.CSIROInterviewApp.Models;
using CSIROInterviewApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CSIROInterviewApp.Controllers
{
    public class EmailController : Controller
    {
        private readonly ApplicationDataContext _context;
        private readonly IConfiguration _configuration;

        public EmailController(ApplicationDataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id.HasValue)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
                if (user is null)
                {
                    return View();
                }
                var content = new StringBuilder();
                content.AppendLine($"Dear {user.Name}");
                content.AppendLine("    Congratulations, you have been selected into our company.");
                content.AppendLine("    \"Regards");
                content.AppendLine("    Hiring Manager\"");

                var model = new EmailViewModel
                {
                    To = user.Email,
                    Subject = "Interview invitation letter",
                    Body = content.ToString()
                };

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = new SmtpClient();
                client.Host = _configuration.GetValue<string>("MailSetting:Host");
                client.Port = _configuration.GetValue<int>("MailSetting:Port");
                var message = new MailMessage();
                message.From = new MailAddress(_configuration.GetValue<string>("MailSetting:Sender"));

                if (client.Host == "smtp.gmail.com")
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential
                    {
                        UserName = _configuration.GetValue<string>("MailSetting:Username"),
                        Password = _configuration.GetValue<string>("MailSetting:Password")
                    };
                }

                foreach (var email in model.To.Split(";"))
                {
                    message.To.Add(email);
                }

                if (model.Cc is not null)
                {
                    foreach (var email in model.Cc?.Split(";"))
                    {
                        message.To.Add(email);
                    }
                }

                message.Subject = model.Subject;
                message.Body = model.Body;

                client.Send(message);
                TempData["SenMailStatus"] = "Send mail successful";

                return RedirectToAction("Index", "Admin");
            }

            return View(model);
        }
    }
}
