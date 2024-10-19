using Microsoft.AspNetCore.Mvc;
using CSIROInterviewApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CSIROInterviewApp.Controllers
    {
    public class GPAFilterController : Controller
        {
        private readonly ApplicationDataContext _context;

        public GPAFilterController(ApplicationDataContext context)
            {
            _context = context;
            }

        // GET: Filtered User List by GPA
        [HttpGet]
        public async Task<IActionResult> Index(float? threshold)
            {
            // Retrieve all users, filtering by the optional GPA threshold if provided
            var users = await _context.Users
                .Include(u => u.Course)
                .Include(u => u.University)
                .Where(u => !threshold.HasValue || u.GPA >= threshold)
                .ToListAsync();

            return View(users);  // Return the view with the filtered user list
            }

        // GET: Create a new GPA Filter
        [HttpGet]
        public IActionResult Create()
            {
            return View();  // Return the view for creating a GPA filter
            }

        // POST: Create a new GPA Filter
        [HttpPost]
        public async Task<IActionResult> Create(GPAFilter model)
            {
            if (ModelState.IsValid)
                {
                // Save the GPA filter in the database
                _context.GPAFilters.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { threshold = model.GPAThreshold });
                // Redirect to Index with the GPA threshold applied
                }

            return View(model);  // Return the view with validation errors
            }

        // GET: Edit GPA Filter
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
            {
            // Retrieve the GPA filter by ID
            var filter = await _context.GPAFilters.FindAsync(id);
            if (filter == null) return NotFound();

            return View(filter);  // Return the view with the filter details for editing
            }

        // POST: Edit GPA Filter
        [HttpPost]
        public async Task<IActionResult> Edit(GPAFilter model)
            {
            if (ModelState.IsValid)
                {
                // Update the GPA filter in the database
                _context.GPAFilters.Update(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");  // Redirect to the filtered user list
                }

            return View(model);  // Return the view with validation errors
            }

        // GET: Delete GPA Filter
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
            {
            var filter = await _context.GPAFilters.FindAsync(id);
            if (filter == null) return NotFound();

            return View(filter);  // Return the confirmation view for deletion
            }

        // POST: Confirm Deletion of GPA Filter
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            var filter = await _context.GPAFilters.FindAsync(id);
            if (filter != null)
                {
                _context.GPAFilters.Remove(filter);
                await _context.SaveChangesAsync();
                }

            return RedirectToAction("Index");  // Redirect to the list after deletion
            }
        }
    }
