namespace VakaxaIDServer.Quickstart.Account
{
    public class VerifyCodeViewModel
    {
        public string ReturnUrl { get; set; }
        public string Code { get; set; }
        public bool RememberMe { get; set; }
        public string Email { get; set; }
        public bool IsGoogleAuthenticator { get; set; }
    }
}