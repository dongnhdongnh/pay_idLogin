using System.ComponentModel.DataAnnotations.Schema;

namespace VakaxaIDServer.Models
{
    [Table("UserActionLog")]
    public class LogActionModel
    {
        public string Id { get; set; }
        public string ActionName { get; set; }
        public string Description { get; set; }
        public string Ip { get; set; }
        public string Location { get; set; }
        public string UserId { get; set; }
        public string Source { get; set; }
        public int CreatedAt { get; set; }

    }
}