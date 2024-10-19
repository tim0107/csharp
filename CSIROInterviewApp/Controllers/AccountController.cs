using Microsoft.AspNetCore.Mvc;
using CSIROInterviewApp.Models;
using CSIROInterviewApp.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System;
using CSIROInterviewApp.Models.CSIROInterviewApp.Models;

namespace CSIROInterviewApp.Controllers
    {
    public class AccountController : Controller
        {
        private readonly ApplicationDataContext _context;

        public AccountController(ApplicationDataContext context)
            {
            _context = context;
            }

        // GET: User Profile
        [HttpGet]
        public async Task<IActionResult> Profile(int id)
            {
            var user = await _context.Users
                .Include(u => u.Course)
                .Include(u => u.University)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null) return NotFound();

            var viewModel = new UserProfileViewModel
                {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                GPA = user.GPA,
                CourseName = user.Course?.CourseName,
                UniversityName = user.University?.UniversityName
                };

            return View(viewModel); 
            }

        // GET: Register a new user
        [HttpGet]
        public IActionResult Register()
            {
            return View(); 
            }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("", "Email already in use.");
                    return View(model);
                }

                string coverLetterPath = null;
                string resumeFilePath = null;

                if (model.CoverLetterFile != null)
                {
                    
                    var coverLetterDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "coverletters");
                    Directory.CreateDirectory(coverLetterDirectory);  
                    var coverLetterFileName = Path.GetFileName(model.CoverLetterFile.FileName);
                    coverLetterPath = Path.Combine(coverLetterDirectory, coverLetterFileName);

                    using (var stream = new FileStream(coverLetterPath, FileMode.Create))
                    {
                        await model.CoverLetterFile.CopyToAsync(stream);
                    }
                }

                if (model.ResumeFile != null)
                {
                    
                    var resumeDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "resumes");
                    Directory.CreateDirectory(resumeDirectory);  
                    var resumeFileName = Path.GetFileName(model.ResumeFile.FileName);
                    resumeFilePath = Path.Combine(resumeDirectory, resumeFileName);

                    using (var stream = new FileStream(resumeFilePath, FileMode.Create))
                    {
                        await model.ResumeFile.CopyToAsync(stream);
                    }
                }
                var university = await _context.Universities.FirstOrDefaultAsync(u => u.UniversityName == model.University);
                var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseName == model.SelectedCourse);


                var user = new User
                {
                    Name = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    GPA = (float)model.GPA,
                    University =  university,
                    PasswordHash = HashPassword(model.Password),
                    Course = course,
                    CoverLetter = coverLetterPath,  
                    ResumeFilePath = resumeFilePath
                    
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }


        
        [HttpGet]
        public IActionResult Login()
            {
            return View(); 
            }

        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
            {
            if (ModelState.IsValid)
                {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email);

                if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
                    {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(model); 
                    }

                return RedirectToAction("Profile", new { id = user.UserId }); 
                }

            return View(model); 
            }

        // GET: Edit User Profile
        [HttpGet]
        public async Task<IActionResult> EditProfile(int id)
            {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var viewModel = new EditProfileViewModel
                {
                UserId = user.UserId,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                GPA = user.GPA,
                CoverLetter = user.CoverLetter,
                ResumeFilePath = user.ResumeFilePath
                };

            return View(viewModel); // Return the edit profile view
            }

        // POST: Edit User Profile
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
            {
            if (ModelState.IsValid)
                {
                var user = await _context.Users.FindAsync(model.UserId);
                if (user == null) return NotFound();

                user.Name = model.Name;
                user.PhoneNumber = model.PhoneNumber;
                user.GPA = (float)model.GPA;
                user.CoverLetter = model.CoverLetter;
                user.ResumeFilePath = model.ResumeFilePath;

                await _context.SaveChangesAsync();
                return RedirectToAction("Profile", new { id = user.UserId }); 
                }

            return View(model); 
            }

       
        public IActionResult ChangePassword(int id)
        {
           
            var model = new ChangePasswordViewModel
            {
                UserId = id,
                OldPassword = string.Empty,  
                NewPassword = string.Empty,  
                ConfirmPassword = string.Empty 
            };
            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
            {
            if (ModelState.IsValid)
                {
                var user = await _context.Users.FindAsync(model.UserId);
                if (user == null) return NotFound();

                if (!VerifyPassword(model.OldPassword, user.PasswordHash))
                    {
                    ModelState.AddModelError("", "Old password is incorrect.");
                    return View(model); 
                    }

                user.PasswordHash = HashPassword(model.NewPassword);
                await _context.SaveChangesAsync();

                return RedirectToAction("Profile", new { id = user.UserId }); 
                }

            return View(model); 
            }

        
        private string HashPassword(string password)
            {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
            }

        
        private bool VerifyPassword(string inputPassword, string storedHash)
            {
            var inputHash = HashPassword(inputPassword);
            return inputHash == storedHash;
            }
        }
    }
