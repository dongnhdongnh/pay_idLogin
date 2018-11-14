using System.ComponentModel.DataAnnotations.Schema;
using VakaxaIDServer.Models.Domains;


namespace VakaxaIDServer.Models.Entities
{
    [Table("EmailQueue")]
    public class EmailQueue : MultiThreadUpdateEntity
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string LinkEmail { get; set; }
        public int Template { get; set; }
    }
}