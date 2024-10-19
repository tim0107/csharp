using CSIROInterviewApp.Models.CSIROInterviewApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.Models
{


    //the purpose of this class manages the activites that occur within the app
    //this will keep score of what an admin or user did i.e
    //admin invited users to an interview
    //Action types: login, sorted Application, updated an application
    public class ActionLog
    {
        [Key]
        public required int LogId { get; set; }

        [ForeignKey("User")]
        public required int? UserId { get; set; } 
        public required User User { get; set; }

        [ForeignKey("Admin")]
        public required int? AdminId { get; set; }
        public required Admin Admin { get; set; }

        [Required]
        [StringLength(100)]
        public required string ActionType { get; set; }

        public required DateTime Timestamp { get; set; }
    }

}
