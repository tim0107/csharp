using System;
using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.ViewModel
{
    public class InterviewViewModel
    {
        
        public int Id { get; set; }

        
        [Required(ErrorMessage = "Applicant name is required")]
        public required string ApplicantName { get; set; }

        
        [Required(ErrorMessage = "Interview date is required")]
        public DateTime InterviewDate { get; set; }

        
        [Required(ErrorMessage = "Interview status is required")]
        public required string Status { get; set; }
    }
}
