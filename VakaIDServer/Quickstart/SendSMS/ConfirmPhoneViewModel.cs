using System.ComponentModel.DataAnnotations;

namespace VakaxaIDServer.Quickstart.SendSMS
{
    public class ConfirmPhoneViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [Display(Name = "ReturnUrl")] public string ReturnUrl { get; set; }
        
        [Required]
        [Display(Name = "PhoneNational")]
        public string PhoneNational { get; set; }
        
        [Required]
        [Display(Name = "CallingCode")]
        public string CallingCode { get; set; }
        
        [Required]
        [Display(Name = "CountryIndex")]
        public string CountryIndex { get; set; }
        
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
    }
}