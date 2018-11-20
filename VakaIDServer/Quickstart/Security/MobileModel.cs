namespace VakaxaIDServer.Quickstart.Security
{
    public class MobileModel
    {
        public const string TypeAddPhoneNumber = "Add PhoneNumber";
        public const string TypeConfirmPhoneNumber = "Confirm PhoneNumber";
        public const string TypeChangePhoneNumber = "Change PhoneNumber";
        public const string TypeEnableTwoFaSms = "EnableTwoFaSms";
        public const string TypeChangeTwoFaSmsToGoogle = "TypeChangeTwoFaSmsToGoogle";
        public const string TypeChangeTwoFaGoogleToSms = "TypeChangeTwoFaGoogleToSms";
        public const string TypeEnableTwoFaGoogle = "EnableTwoFaGoogle";
        public const string TypeDisableTwoFa = "DisableTwoFa";

        public string TabShow { get; set; } = "";
        public bool IsTabSelected { get; set; }
        public int Status { get; set; } = SecurityViewModel.StatusDefault;
        public string Message { get; set; } ="";
        public string PhoneHide { get; set; }
        public string CountryCode { get; set; }
        public string CallingCode { get; set; }
        public string PhoneNational { get; set; }
        public bool Confirmed { get; set; }
        public bool IsTwoFaSms { get; set; }
        public bool IsTwoFaGoogle { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
    }
}