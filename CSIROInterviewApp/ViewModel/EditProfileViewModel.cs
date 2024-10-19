using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.ViewModel
{
    public class EditProfileViewModel
    {
        [Required]
        public required int UserId { get; set; }  

        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "GPA is required")]
        [Range(0.0, 4.0, ErrorMessage = "GPA must be between 0.0 and 4.0")]
        public required double GPA { get; set; }

        [Required(ErrorMessage = "Cover letter is required")]
        public required string CoverLetter { get; set; }

        [Required(ErrorMessage = "Resume file path is required")]
        public required string ResumeFilePath { get; set; }
    }
}
