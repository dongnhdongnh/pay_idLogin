using System.ComponentModel.DataAnnotations;

namespace VakaxaIDServer.Quickstart.Security
{
    public class ChangePassModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,100}$", ErrorMessage =
            "Passwords must be at least 8 and most 100 characters and at least one letter, one number and one special character")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,100}$", ErrorMessage =
            "Passwords must be at least 8 and most 100 characters and at least one letter, one number and one special character")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Code")]
        [RegularExpression(@"^(?=.*[0-9]){6}$", ErrorMessage = "Code must be 6 digits.")]
        public string Code { get; set; }

        public string TabShow { get; set; } = "";
        public bool IsTabSelected { get; set; } = false;
        public int Status { get; set; } = SecurityViewModel.StatusDefault;
    }
}