using CSIROInterviewApp.Models.CSIROInterviewApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.Models
{
    public class Invitation
    {
        [Key]
        public int InvitationId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public required User User { get; set; }

        public DateTime SentDate { get; set; }
        public DateTime? InterviewDate { get; set; } 

        [StringLength(50)]
        public required string Status { get; set; } // "Sent", "Accepted", "Rejected"
    }

}
