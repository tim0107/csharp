namespace CSIROInterviewApp.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace CSIROInterviewApp.Models
    {
        public class Admin
        {
           
                [Key]
                public int AdminId { get; set; }

                [Required]
                [StringLength(100)]
                public required string Name { get; set; }

                [Required]
                [EmailAddress]
                [StringLength(100)]
                public required string Email { get; set; }

                public required string PasswordHash { get; set; }

                // Navigation property for AdminApplications
                public required ICollection<AdminApplication> AdminApplications { get; set; }
            }











            /*  public int adminId { get; set; } // Primary key
              public string firstName { get; set; } // First Name
              public string lastName { get; set; }  // Last Name                           
              public string email { get; set; } // Email address
              public string PpsswordHash { get; set; } // Encrypted password*/
        }

    }


