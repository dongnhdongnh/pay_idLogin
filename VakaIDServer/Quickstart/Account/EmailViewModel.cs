namespace VakaxaIDServer.Quickstart.Account
{
    public class EmailViewModel
    {
        public string Email { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string ReturnUrl { get; set; }
        public int TypeVerify { get; set; }
    }
}