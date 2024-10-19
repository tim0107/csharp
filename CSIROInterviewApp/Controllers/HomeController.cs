using Microsoft.AspNetCore.Mvc;
using CSIROInterviewApp.ViewModel;

namespace CSIROInterviewApp.Controllers
    {
    public class HomeController : Controller
        {
       
        [HttpGet]
        public IActionResult Index()
            {

            
            return View();
            }
        }
    }
