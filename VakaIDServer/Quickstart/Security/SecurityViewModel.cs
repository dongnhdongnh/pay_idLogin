namespace VakaxaIDServer.Quickstart.Security
{
    public class SecurityViewModel
    {
        public const int StatusDefault = 0;
        public const int StatusSuccess = 1;
        public const int StatusError = 2;
        
        public string Email { get; set; }
        public bool TwoFactorEnable { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsGoogleAuthenticator { get; set; }
        public int Tab { get; set; }
        public ChangePassModel ChangePassModel { get; set; }
        public MobileModel MobileModel { get; set; }
        public LockScreenModel LockScreenModel { get; set; }
        public DeactiveModel DeactiveModel { get; set; }
        public EnableAuthenticatorViewModel Authenticator { get; set; }
    }
}