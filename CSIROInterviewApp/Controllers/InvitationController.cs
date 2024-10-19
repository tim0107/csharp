using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CSIROInterviewApp.Models;

using System;

namespace CSIROInterviewApp.Controllers
    {
    public class InvitationController : Controller
        {
        private readonly ApplicationDataContext _context;

        public InvitationController(ApplicationDataContext context)
            {
            _context = context;
            }

        // GET: List of Invitations
        [HttpGet]
        public async Task<IActionResult> Index()
            {
            // Retrieve the list of invitations with user details
            var invitations = await _context.Invitations
                .Include(i => i.User)
                .ToListAsync();

            return View(invitations); 
            }

        
        [HttpGet]
        public IActionResult Create()
            {
            return View(); 
            }

        // POST: Create a new Invitation
        [HttpPost]
        public async Task<IActionResult> Create(Invitation model)
            {
            if (ModelState.IsValid)
                {
                
                model.SentDate = DateTime.Now;

               
                _context.Invitations.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index"); 
                }

            return View(model); 
            }

        // GET: Edit Invitation details
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
            {
            // Retrieve the invitation by ID
            var invitation = await _context.Invitations.FindAsync(id);
            if (invitation == null) return NotFound();

            return View(invitation); // Return the Edit Invitation view
            }

        // POST: Edit Invitation details
        [HttpPost]
        public async Task<IActionResult> Edit(Invitation model)
            {
            if (ModelState.IsValid)
                {
                // Find the invitation to update
                var invitation = await _context.Invitations.FindAsync(model.InvitationId);
                if (invitation == null) return NotFound();

                // Update the invitation details
                invitation.InterviewDate = model.InterviewDate;
                invitation.Status = model.Status;

                await _context.SaveChangesAsync();

                return RedirectToAction("Index"); // Redirect to list after editing
                }

            return View(model); // Return view with validation errors
            }

        // GET: Delete Invitation
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
            {
            // Find the invitation to delete
            var invitation = await _context.Invitations.FindAsync(id);
            if (invitation == null) return NotFound();

            return View(invitation); // Return the Delete Confirmation view
            }

        // POST: Confirm Deletion of Invitation
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
            // Find the invitation to remove
            var invitation = await _context.Invitations.FindAsync(id);
            if (invitation != null)
                {
                _context.Invitations.Remove(invitation);
                await _context.SaveChangesAsync();
                }

            return RedirectToAction("Index"); // Redirect to list after deletion
            }
        }
    }
