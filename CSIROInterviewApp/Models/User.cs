namespace CSIROInterviewApp.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace CSIROInterviewApp.Models
    {

        public class User
        {
            [Key]
            public int UserId { get; set; }

            [Required]
            [StringLength(100)]
            public required string Name { get; set; }

            [Required]
            [EmailAddress]
            [StringLength(100)]
            public required string Email { get; set; }

            [Phone]
            public required string PhoneNumber { get; set; }

            [ForeignKey("Course")]
            public int CourseId { get; set; }
            public required Course Course { get; set; }

            [Range(0, 4.0)]
            public required float GPA { get; set; }

            [ForeignKey("University")]
            public int UniversityId { get; set; }
            public required University University { get; set; }

            public required string CoverLetter { get; set; }
            public required string ResumeFilePath { get; set; }

            public required string PasswordHash { get; set; }

            public ICollection<Application> Applications { get; set; }

            public ICollection<Invitation> Invitations { get; set; }
        }





        /* public int userId { get; set; } // Primary key
         public string firstName { get; set; } // First Name

         public string lastName { get; set; }  // Last Name
         public string email { get; set; } // Email address
         public string phoneNumber { get; set; } // Contact number
         public int courseId { get; set; } // Foreign key referencing Course
         public double GPA { get; set; } // User's GPA
         public int universityId { get; set; } // Foreign key referencing University
         public string coverLetter { get; set; } // Cover letter text
         public string resumeFilePath { get; set; } // Path to resume
         public string passwordHash { get; set; } // Encrypted password
         public string role { get; set; } // Role ("User" or "Admin") */
    }


}
