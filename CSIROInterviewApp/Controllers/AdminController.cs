using Microsoft.AspNetCore.Mvc;
using CSIROInterviewApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSIROInterviewApp.Models.CSIROInterviewApp.Models;
using CSIROInterviewApp.ViewModel;
using System;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Authorization;


namespace CSIROInterviewApp.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDataContext _context;
        private readonly IConfiguration _configuration;

        public AdminController(ApplicationDataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Admin List
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.Select(u => new UserDetail
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Course = u.Course.CourseName,
                GPA = u.GPA,
                University = u.University.UniversityName,
                CoverLetterFilePath = u.CoverLetter,
                ResumeFilePath = u.ResumeFilePath,
            }).ToListAsync();

            users.ForEach(user =>
            {
                user.CoverLetterFilePath = GetFileNameFromPath(user.CoverLetterFilePath);
                user.ResumeFilePath = GetFileNameFromPath(user.ResumeFilePath);
            });

            var model = new UserViewModel
            {
                Users = users
            };

            return View(model); // Return the view with the list of admins
        }

        public async Task<IActionResult> Filter(float gpa)
        {
            if (gpa is 0)
            {
                return RedirectToAction("Index");
            }

            var users = await _context.Users
                .Where(u => u.GPA == gpa)
                .Select(u => new UserDetail
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Course = u.Course.CourseName,
                    GPA = u.GPA,
                    University = u.University.UniversityName,
                    CoverLetterFilePath = u.CoverLetter,
                    ResumeFilePath = u.ResumeFilePath,
                }).ToListAsync();

            users.ForEach(user =>
            {
                user.CoverLetterFilePath = GetFileNameFromPath(user.CoverLetterFilePath);
                user.ResumeFilePath = GetFileNameFromPath(user.ResumeFilePath);
            });

            var model = new UserViewModel
            {
                GPA = gpa,
                Users = users
            };

            return View("Index", model); // Return the view with the list of admins
        }

        [HttpGet]
        [Route("admin/send-invitation/{id}")]
        public IActionResult SendInvitation(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user is null)
            {
                return NotFound();
            }

            var client = new SmtpClient();
            client.Host = _configuration.GetValue<string>("MailSetting:Host");
            client.Port = _configuration.GetValue<int>("MailSetting:Port");
            var message = new MailMessage();
            message.From = new MailAddress(_configuration.GetValue<string>("MailSetting:Sender"));
            message.To.Add(user.Email);
            var content = new StringBuilder();
            content.AppendLine($"Dear {user.Name}");
            content.AppendLine("    Congratulations, you have been selected into our company.");
            content.AppendLine("    \"Regards");
            content.AppendLine("    Hiring Manager\"");
            message.Subject = "Interview invitation letter";
            message.Body = content.ToString();

            client.Send(message);
            TempData["SenMailStatus"] = "Send mail successful";

            return RedirectToAction("Index");
        }

        // GET: Register an Admin
        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Return the registration view for admins
        }

        [HttpPost]
        public async Task<IActionResult> Register(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("Password", "Password is required.");
                    return View(model);
                }

                var admin = new Admin
                {
                    Name = model.Name,
                    Email = model.Email,
                    PasswordHash = HashPassword(model.Password),
                    AdminApplications = new List<AdminApplication>()
                };

                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model); // Return view with validation errors
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.AdminId == id);
            if (admin == null) return NotFound();

            var model = new AdminViewModel
            {
                AdminId = admin.AdminId,
                Name = admin.Name,
                Email = admin.Email,
                Password = string.Empty

            };

            return View(model);
        }

        // POST: Edit Admin Details
        [HttpPost]
        public async Task<IActionResult> Edit(AdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _context.Admins.FirstOrDefault(a => a.AdminId == model.AdminId);
                if (admin == null) return NotFound();

                admin.Name = model.Name;
                admin.Email = model.Email;

                await _context.SaveChangesAsync();

                return RedirectToAction("Index"); // Redirect to admin list after update
            }

            return View(model); // Return view with validation errors
        }

        // GET: Delete Admin
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.AdminId == id);
            if (admin == null) return NotFound();

            return View(admin); // Confirm deletion
        }

        // POST: Delete Admin
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var admin = _context.Admins.FirstOrDefault(a => a.AdminId == id);
            if (admin == null) return NotFound();

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // Redirect to admin list after deletion
        }

        // Utility: Hash Password
        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);

            return System.Convert.ToBase64String(hash);
        }

        private string GetFileNameFromPath(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return null;
            }

            var fileInfo = new FileInfo(path);

            return fileInfo.Name;
        }
    }
}
