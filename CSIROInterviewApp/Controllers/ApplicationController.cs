using Microsoft.AspNetCore.Mvc;
using CSIROInterviewApp.ViewModel;
using CSIROInterviewApp.Models;



namespace CSIROInterviewApp.Controllers
{
   

    public class ApplicationController : Controller
    {
        private readonly ApplicationDataContext _context;

        public ApplicationController(ApplicationDataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            // Retrieve the list of applications from the database
            var applications = _context.Applications
                .Select(a => new ApplicationViewModel
                {
                    Id = a.ApplicationId,
                    UserName = a.User.Name,  
                    CourseName = a.Course.CourseName, 
                    Status = a.Status
                }).ToList();

            return View(applications);  
        }

        // GET: Create a new application
        [HttpGet]
        public IActionResult Create()
        {
            return View();  
        }

        // POST: Create a new application
        [HttpPost]
        public IActionResult Create(ApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Logic to add the new application to the database

                return RedirectToAction("Index");  // Redirect to application list after successful creation
            }

            return View(model);  // Return the view with validation errors
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Retrieve the application data from the database using the ID
            var application = _context.Applications
                .Where(a => a.ApplicationId == id)
                .Select(a => new ApplicationViewModel
                {
                    Id = a.ApplicationId,
                    UserName = a.User.Name,  // Assuming User is a related entity
                    CourseName = a.Course.CourseName,  // Assuming Course is a related entity
                    Status = a.Status
                }).FirstOrDefault();

            if (application == null) return NotFound();

            return View(application);  // Return the edit view with the application details
        }

        // POST: Edit an application
        [HttpPost]
        public IActionResult Edit(ApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Logic to update the application details in the database

                return RedirectToAction("Index");  // Redirect to application list after successful update
            }

            return View(model);  // Return the view with validation errors
        }
    }
}
