using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.ViewModel
{
    public class UserProfileViewModel
    {
        public required int UserId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public required string PhoneNumber { get; set; }

        [Range(0.0, 4.0)]
        public required double GPA { get; set; }

        public required string CourseName { get; set; }

        public required string UniversityName { get; set; }
    }
}
