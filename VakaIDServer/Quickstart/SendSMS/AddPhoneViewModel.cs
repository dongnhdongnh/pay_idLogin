using System.ComponentModel.DataAnnotations;

namespace VakaxaIDServer.Quickstart.SendSMS
{
    public class AddPhoneViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [Display(Name = "ReturnUrl")] public string ReturnUrl { get; set; }
        
        [Required]
        [Display(Name = "PhoneNational")]
        public string PhoneNational { get; set; }
        
        [Display(Name = "CountryCode")]
        public string CountryCode { get; set; }
        
        [Required]
        [Display(Name = "CallingCode")]
        public string CallingCode { get; set; }
        
        [Display(Name = "CountryIndex")]
        public int CountryIndex { get; set; }
        
        [Display(Name = "PhoneNumber")] public string PhoneNumber { get; set; }
    }
}