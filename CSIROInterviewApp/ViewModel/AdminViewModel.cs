using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.ViewModel
{
    public class AdminViewModel
    {


        public  int? AdminId { get; set; }  



        
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }


        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public required string Password { get; set; }
    }
}
