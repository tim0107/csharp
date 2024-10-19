using Microsoft.AspNetCore.Mvc;
using CSIROInterviewApp.ViewModel;
using CSIROInterviewApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography; 
using System.Text;
using CSIROInterviewApp.Models.CSIROInterviewApp.Models;

namespace CSIROInterviewApp.Controllers
{
    public class UserController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ApplicationDataContext _context;

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public UserController(ApplicationDataContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                Username = string.Empty,  
                GPA = 0.0,  
                University = string.Empty, 
                Email = string.Empty,
                Password = string.Empty,  
                ConfirmPassword = string.Empty,  
                SelectedCourse = string.Empty,
                Courses = new List<string>
                {
                    "Master of Data Science",
                    "Master of Artificial Intelligence",
                    "Master of Information Technology",
                    "Master of Science (Statistics)"
                },
                ResumeFile = string.Empty,
                CoverLetterFile = string.Empty
            };

            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var selectedCourse = await _context.Courses
                    .FirstOrDefaultAsync(c => c.CourseName == model.SelectedCourse);

                if (selectedCourse == null)
                {
                    ModelState.AddModelError("", "Selected course not found.");
                    return View(model);
                }
                var university = await _context.Universities
                  .FirstOrDefaultAsync(u => u.UniversityName == model.University);


                //handle resume and cover letter

                
                var coverLetterPath = null;
                var resumeFilePath = null;

                if (model.CoverLetterFile != null)
                {
                    
                    var coverLetterFileName = Path.GetFileName(model.CoverLetterFile.FileName);
                    var coverLetterUploadPath = Path.Combine("Uploads", "CoverLetters", coverLetterFileName);
                    using (var stream = new FileStream(coverLetterUploadPath, FileMode.Create))
                    {
                        await model.CoverLetterFile.CopyToAsync(stream);
                    }
                    coverLetterPath = coverLetterUploadPath;
                }

                if (model.ResumeFile != null)
                {
                    
                    var resumeFileName = Path.GetFileName(model.ResumeFile.FileName);
                    var resumeUploadPath = Path.Combine("Uploads", "Resumes", resumeFileName);
                    using (var stream = new FileStream(resumeUploadPath, FileMode.Create))
                    {
                        await model.ResumeFile.CopyToAsync(stream);
                    }
                    resumeFilePath = resumeUploadPath;
                }

                var user = new User
                {
                    Name = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CoverLetter =  coverLetterPath,
                    ResumeFilePath = resumeFilePath,
                    GPA = (float)model.GPA,
                    PasswordHash = HashPassword(model.Password),
                    Course = selectedCourse,
                    University = university,

                    Applications = new List<Application>(),
                    Invitations = new List<Invitation>()


                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }


        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            
            var student = new EditViewModel
            {
                University = "Sample University",
                GPA = 3.5,
                SelectedCourse = "Master of Data Science",
                Courses = new List<string>
                {
                    "Master of Data Science",
                    "Master of Artificial Intelligence",
                    "Master of Information Technology",
                    "Master of Science (Statistics)"
                }
            };

            return View(student);  
        }

        
        [HttpPost]
        public IActionResult Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                

                return RedirectToAction("Profile", "User");  
            }

            
            model.Courses = new List<string>
            {
                "Master of Data Science",
                "Master of Artificial Intelligence",
                "Master of Information Technology",
                "Master of Science (Statistics)"
            };

            return View(model);  
        }

        
        [HttpGet]
        public IActionResult Profile()
        {
            
            return View();  
        }
    }
}





