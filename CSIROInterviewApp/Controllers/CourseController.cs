using Microsoft.AspNetCore.Mvc;
using CSIROInterviewApp.ViewModel;
using CSIROInterviewApp.Models;


namespace CSIROInterviewApp.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDataContext _context;

        public CourseController(ApplicationDataContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public IActionResult Index()
        {
            // Logic to retrieve the list of courses from the database
            var courses = new List<CourseViewModel>
            {
                new CourseViewModel { CourseId = 1, CourseName = "Master of Data Science" },
                new CourseViewModel { CourseId = 2, CourseName = "Master of Artificial Intelligence" },
                new CourseViewModel { CourseId = 3, CourseName = "Master of Information Technology" }
            };

            return View(courses);  
        }

        
        [HttpGet]
        public IActionResult Create()
        {
            return View();  
        }

        
        [HttpPost]
        public async Task<IActionResult> AddCourse(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var course = new Course
                {
                    CourseName = model.CourseName

                };

                _context.Courses.Add(course);  
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(model);  
        }


        
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // In a real application, retrieve the course data from the database using the ID
            var course = new CourseViewModel
            {
                CourseId = id,
                CourseName = "Master of Data Science"
            };

            return View(course);  // Return the edit view with the course details
        }

        // POST: Edit a course
        [HttpPost]
        public IActionResult Edit(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Logic to update the course details in the database

                return RedirectToAction("Index");  // Redirect to course list after successful update
            }

            return View(model);  // Return the view with validation errors
        }
    }
}
