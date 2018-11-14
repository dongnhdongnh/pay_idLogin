using VakaxaIDServer.Models.Domains;

namespace VakaxaIDServer.Models.Entities
{
    public class SmsQueue : MultiThreadUpdateEntity
    {
        public string TextSend { get; set; }
        public string To { get; set; }
    }
}