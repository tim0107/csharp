using CSIROInterviewApp.Models.CSIROInterviewApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.Models
{
    public class University
    {
    
            [Key]
            public int UniversityId { get; set; }

            [Required]
            [StringLength(100)]
            public required string UniversityName { get; set; }

            public int Ranking { get; set; }

            // Navigation property for Users
            public required ICollection<User> Users { get; set; }
        }

    }

