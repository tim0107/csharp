using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.ViewModel
{
    public class EmailViewModel
    {
        [Required]
        public string To { get; set; }
        public string? Cc { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
