using System.ComponentModel.DataAnnotations.Schema;


namespace VakaxaIDServer.Models
{
    [Table("EmailQueue")]
    public class EmailQueueModel
    {
        public string Id { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string LinkEmail { get; set; }
        public int Template { get; set; }
        public int IsProcessing { get; set; }
        public int Version { get; set; }
        public string Status { get; set; }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
    }
}