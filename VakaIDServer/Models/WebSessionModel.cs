using System.ComponentModel.DataAnnotations.Schema;

namespace VakaxaIDServer.Models
{
    [Table("WebSessionModel")]
    public class WebSessionModel
    {
        public string Id { get; set; }
        public bool Current { get; set; }
        public string Browser { get; set; }
        public string Ip { get; set; }
        public string UserId { get; set; }
        public int SignedIn { get; set; }
        public string Location { get; set; }
    }
}