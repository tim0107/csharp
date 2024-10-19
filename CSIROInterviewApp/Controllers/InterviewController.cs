using Microsoft.AspNetCore.Mvc;
using CSIROInterviewApp.ViewModel;

namespace CSIROInterviewApp.Controllers
{
    public class InterviewController : Controller
    {
        // GET: Interview List
        [HttpGet]
        public IActionResult Index()
        {
            
            var interviews = new List<InterviewViewModel>
            {
                new InterviewViewModel { Id = 1, ApplicantName = "John Doe", InterviewDate = DateTime.Now, Status = "Scheduled" },
                new InterviewViewModel { Id = 2, ApplicantName = "Jane Smith", InterviewDate = DateTime.Now.AddDays(1), Status = "Completed" }
            };

            return View(interviews);  
        }

        // GET: Schedule an interview
        [HttpGet]
        public IActionResult Schedule()
        {
            return View();  
        }

        // POST: Schedule an interview
        [HttpPost]
        public IActionResult Schedule(InterviewViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Logic to schedule a new interview in the database

                return RedirectToAction("Index");  
            }

            return View(model);  
        }

        // GET: Edit interview details
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // In a real application, retrieve the interview data from the database using the ID
            var interview = new InterviewViewModel
            {
                Id = id,
                ApplicantName = "John Doe",
                InterviewDate = DateTime.Now,
                Status = "Scheduled"
            };

            return View(interview); 
        }

        // POST: Edit interview details
        [HttpPost]
        public IActionResult Edit(InterviewViewModel model)
        {
            if (ModelState.IsValid)
            {
               

                return RedirectToAction("Index"); 
            }

            return View(model);  // Return the view with validation errors
        }
    }
}
