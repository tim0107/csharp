using System.ComponentModel.DataAnnotations;

namespace CSIROInterviewApp.ViewModel
{
    public class ChangePasswordViewModel
    {
        [Required]
        public  int UserId { get; set; }  

        [Required(ErrorMessage = "Old password is required")]
        [DataType(DataType.Password)]
        public required string OldPassword { get; set; }  

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public required string NewPassword { get; set; }  

        [Required(ErrorMessage = "Confirm new password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
        public required string ConfirmPassword { get; set; }  
    }
}
