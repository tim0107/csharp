using CSIROInterviewApp.Models.CSIROInterviewApp.Models;
using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.Models
{
    public class Role
    {
      
       
            [Key]
            public int RoleId { get; set; }

            [Required]
            [StringLength(50)]
            public required string RoleName { get; set; }

            
            public required ICollection<User> Users { get; set; }
        }

    }

