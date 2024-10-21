using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.ViewModel
{
    public class EditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Username { get; set; }

        [Required]
        [Range(0.0, 4.0, ErrorMessage = "GPA must be between 0.0 and 4.0")]
        public float GPA { get; set; }

        [Required]
        public string University { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        
        [Display(Name = "Cover Letter file")]
        [DataType(DataType.Upload)]
        public IFormFile? CoverLetterFile { get; set; }

        [Display(Name = "Resume file")]
        [DataType(DataType.Upload)]
        public IFormFile? ResumeFile { get; set; }

        [Required]
        public string SelectedCourse { get; set; }

        public List<string>? Courses { get; set; }

        public string? CurrentCoverLetterFilePath { get; set; }
        public string? CurrentResumeFilePath { get; set; }
    }
}
