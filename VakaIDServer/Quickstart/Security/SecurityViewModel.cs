namespace VakaxaIDServer.Quickstart.Security
{
    public class SecurityViewModel
    {
        public string Email { get; set; }
        public bool TwoFactorEnable { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneHide { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string CountryCode { get; set; }
        public bool IsGoogleAuthenticator { get; set; }
        public EnableAuthenticatorViewModel Authenticator { get; set; }

    }
}