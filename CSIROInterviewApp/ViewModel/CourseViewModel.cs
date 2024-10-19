
using CSIROInterviewApp.Models;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations;  
using System.Collections.Generic;

namespace CSIROInterviewApp.ViewModel
{
    public class CourseViewModel
    {
        [Required(ErrorMessage = "Id is required")]
        public required int CourseId { get; set; }

        [Required(ErrorMessage = "Course name is required")]
        public required string CourseName { get; set; }

    }
}
