namespace CSIROInterviewApp.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace CSIROInterviewApp.Models
    {
        public class Application
        {

                [Key]
                public required int ApplicationId { get; set; }

                [ForeignKey("User")]
                public required int UserId { get; set; }
                public required User User { get; set; }

                [StringLength(50)]
                public required string Status { get; set; } // "Pending", "Accepted", "Rejected"

                public required string CoverLetter { get; set; }
                public required string ResumeFilePath { get; set; }

                public required float GPAThreshold { get; set; }

                [ForeignKey("Course")]
                public required int CourseId { get; set; }
                public required Course Course { get; set; }

                // Navigation property for AdminApplications
                public required ICollection<AdminApplication> AdminApplications { get; set; }
            }









            /* public int applicationId { get; set; } // Primary key
            public int userId { get; set; } // Foreign key referencing User
            public string status { get; set; } // Application status
            public string coverLetter { get; set; } // Cover letter text
            public string resumeFilePath { get; set; } // Path to resume*/
        }

    }


