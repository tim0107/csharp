﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        [Range(0.0, 4.0, ErrorMessage = "GPA must be between 0.0 and 4.0")]
        public required double GPA { get; set; }

        [Required]
        public required string University { get; set; }


        public string PhoneNumber { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public required string Email { get; set; }  

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public required string ConfirmPassword { get; set; }

        public required IFormFile CoverLetterFile { get; set; }  
        public required IFormFile ResumeFile { get; set; }  

        [Required]
        public required string SelectedCourse { get; set; }

        public required List<string> Courses { get; set; } = new List<string>
        {
            "Master of Data Science",
            "Master of Artificial Intelligence",
            "Master of Information Technology",
            "Master of Science (Statistics)"
        };
    }
}
