using Microsoft.AspNetCore.Mvc;
using CSIROInterviewApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSIROInterviewApp.Models.CSIROInterviewApp.Models;
using CSIROInterviewApp.ViewModel;
using System;


namespace CSIROInterviewApp.Controllers
    {
    public class AdminController : Controller
        {
        private readonly ApplicationDataContext _context;

        public AdminController(ApplicationDataContext context)
            {
            _context = context;
            }

        // GET: Admin List
        [HttpGet]
        public async Task<IActionResult> Index()
            {
            var admins = await Task.FromResult(_context.Admins.ToList());

            return View(admins); // Return the view with the list of admins
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
        }
    }
