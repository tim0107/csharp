using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.ViewModel
{
    public class ApplicationViewModel
    {
        public int Id { get; set; }  

        [Required(ErrorMessage = "User name is required")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        public required string CourseName { get; set; }

        public required string Status { get; set; }
    }
}
