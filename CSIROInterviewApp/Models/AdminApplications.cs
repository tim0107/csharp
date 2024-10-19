using CSIROInterviewApp.Models.CSIROInterviewApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.Models
{

    //this model's function is to be used to establish and track which admin sent out invitations and sorted them
    public class AdminApplication
    {
        [Key]
        public required int AdminApplicationId { get; set; }

        [ForeignKey("Admin")]
        public required int AdminId { get; set; }
        public required Admin Admin { get; set; }

        [ForeignKey("Application")]
        public required int ApplicationId { get; set; }
        public required Application Application { get; set; }

        public required DateTime ProcessedDate { get; set; }
    }

}
