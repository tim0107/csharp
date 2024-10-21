using CSIROInterviewApp.Models.CSIROInterviewApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.Models
{
    public class Course
    {

        [Key]
        public int CourseId { get; set; }

        [Required]
        [StringLength(100)]
        public required string CourseName { get; set; }

        // Navigation property for Applications
        public virtual ICollection<User> Applications { get; set; }
    }
}

