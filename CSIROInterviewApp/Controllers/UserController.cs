using Microsoft.AspNetCore.Mvc;
using CSIROInterviewApp.ViewModel;
using CSIROInterviewApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography; 
using System.Text;
using CSIROInterviewApp.Models.CSIROInterviewApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace CSIROInterviewApp.Controllers
{
    [Authorize(Policy = "User")]
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
        [AllowAnonymous]
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
                Courses = _context.Courses.Select(c => c.CourseName).ToList()
            };

            return View(model); 
        }

        [HttpPost]
        [AllowAnonymous]
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

                if (model.GPA < 3 || model.GPA > 4)
                {
                    ModelState.AddModelError(nameof(EditViewModel.GPA), "GPA must be between 3.0 and 4.0");
                    return View(model);
                }
                //handle resume and cover letter


                var coverLetterPath = string.Empty;
                var resumeFilePath = string.Empty;

                if (model.CoverLetterFile != null)
                {
                    
                    var coverLetterFileName = Path.GetFileName(model.CoverLetterFile.FileName);
                    var coverLetterUploadPath = Path.Combine("Uploads", model.Username, "CoverLetters", coverLetterFileName);

                    if (!Directory.Exists(Path.Combine("Uploads", model.Username, "CoverLetters")))
                    {
                        Directory.CreateDirectory(Path.Combine("Uploads", model.Username, "CoverLetters"));
                    }

                    using (var stream = new FileStream(coverLetterUploadPath, FileMode.Create))
                    {
                        await model.CoverLetterFile.CopyToAsync(stream);
                    }
                    coverLetterPath = coverLetterUploadPath;
                }

                if (model.ResumeFile != null)
                {
                    
                    var resumeFileName = Path.GetFileName(model.ResumeFile.FileName);
                    var resumeUploadPath = Path.Combine("Uploads", model.Username, "Resumes", resumeFileName);

                    if (!Directory.Exists(Path.Combine("Uploads", model.Username, "Resumes")))
                    {
                        Directory.CreateDirectory(Path.Combine("Uploads", model.Username, "Resumes"));
                    }

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

                return RedirectToAction("Profile", new {
                    id = user.UserId
                });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.Users
                .Include(u => u.University)
                .Include(u => u.Course)
                .FirstOrDefault(u => u.UserId == id);
            if (user is null)
            {
                return NotFound();
            }
            
            var model = new EditViewModel
            {
                Id = user.UserId,
                Username = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                University = user.University?.UniversityName,
                GPA = user.GPA,
                SelectedCourse = user.Course?.CourseName,
                Courses = _context.Courses.Select(c => c.CourseName).ToList(),
                CurrentCoverLetterFilePath = GetFileNameFromPath(user.CoverLetter),
                CurrentResumeFilePath = GetFileNameFromPath(user.ResumeFilePath)
            };

            return View(model);
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
        public IActionResult Profile(int id)
        {
            var user = _context.Users
                .Include(u => u.University)
                .Include(u => u.Course)
                .FirstOrDefault(u => u.UserId == id);
            if (user is null)
            {
                return NotFound();
            }

            var model = new UserProfileViewModel
            {
                Id = user.UserId,
                Email = user.Email,
                Username = user.Name,
                GPA = user.GPA,
                University = user.University?.UniversityName,
                SelectedCourse = user.Course?.CourseName,
                PhoneNumber = user.PhoneNumber,
                CoverLetterFile = GetFileNameFromPath(user.CoverLetter),
                ResumeFile = GetFileNameFromPath(user.ResumeFilePath)
            };

            return View(model);  
        }

        [HttpPost]
        public async Task<IActionResult> Save(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == model.Id);
                if (user is null)
                {
                    return NotFound();
                }

                #region Update course
                var selectedCourse = await _context.Courses
                    .FirstOrDefaultAsync(c => c.CourseName == model.SelectedCourse);

                if (selectedCourse == null)
                {
                    ModelState.AddModelError(nameof(EditViewModel.SelectedCourse), "Selected course not found.");
                    return View("Edit", model);
                }
                else
                {
                    user.Course = selectedCourse;
                }
                #endregion

                #region Update university
                var university = await _context.Universities
                          .FirstOrDefaultAsync(u => u.UniversityName == model.University);

                if (university is null)
                {
                    ModelState.AddModelError("", "Selected course not found.");
                    return View("Edit", model);
                }
                else
                {
                    user.University = university;
                }
                #endregion

                #region Update GPA
                if (model.GPA >= 3.0 & model.GPA <= 4)
                {
                    user.GPA = model.GPA;
                }
                else
                {
                    ModelState.AddModelError(nameof(EditViewModel.GPA), "GPA must be between 3.0 and 4.0");
                    return View("Edit", model);
                }
                #endregion

                #region Update phone number
                user.PhoneNumber = model.PhoneNumber; 
                #endregion

                #region Update Cover letter file
                if (model.CoverLetterFile != null)
                {
                    var coverLetterFileName = Path.GetFileName(model.CoverLetterFile.FileName);
                    var coverLetterUploadPath = Path.Combine("Uploads", model.Username, "CoverLetters", coverLetterFileName);

                    if (!Directory.Exists(Path.Combine("Uploads", model.Username, "CoverLetters")))
                    {
                        Directory.CreateDirectory(Path.Combine("Uploads", model.Username, "CoverLetters"));
                    }

                    using (var stream = new FileStream(coverLetterUploadPath, FileMode.Create))
                    {
                        await model.CoverLetterFile.CopyToAsync(stream);
                    }
                    user.CoverLetter = coverLetterUploadPath;
                }
                #endregion

                #region Update resume file
                if (model.ResumeFile != null)
                {

                    var resumeFileName = Path.GetFileName(model.ResumeFile.FileName);
                    var resumeUploadPath = Path.Combine("Uploads", model.Username, "Resumes", resumeFileName);

                    if (!Directory.Exists(Path.Combine("Uploads", model.Username, "Resumes")))
                    {
                        Directory.CreateDirectory(Path.Combine("Uploads", model.Username, "Resumes"));
                    }

                    using (var stream = new FileStream(resumeUploadPath, FileMode.Create))
                    {
                        await model.ResumeFile.CopyToAsync(stream);
                    }
                    user.ResumeFilePath = resumeUploadPath;
                } 
                #endregion

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Update user successful";

                return RedirectToAction("Profile", new
                {
                    id = user.UserId
                });
            }

            return View("Edit", model);
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





