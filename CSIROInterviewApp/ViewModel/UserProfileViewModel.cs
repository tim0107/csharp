using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.ViewModel
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public required string Username { get; set; }

        [Required]
        [Range(0.0, 4.0, ErrorMessage = "GPA must be between 0.0 and 4.0")]
        public required float GPA { get; set; }

        [Required]
        public required string University { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }

        [Required]
        [Display(Name = "Cover Letter file")]
        public string CoverLetterFile { get; set; }

        [Required]
        [Display(Name = "Resume file")]
        public string ResumeFile { get; set; }

        [Required]
        public required string SelectedCourse { get; set; }
    }
}
