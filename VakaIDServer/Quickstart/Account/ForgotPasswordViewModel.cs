using System.ComponentModel.DataAnnotations;

namespace VakaxaIDServer.Quickstart.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}