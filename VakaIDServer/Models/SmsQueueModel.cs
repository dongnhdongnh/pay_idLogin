using System.ComponentModel.DataAnnotations.Schema;


namespace VakaxaIDServer.Models
{
    [Table("SmsQueue")]
    public class SmsQueueModel
    {
        public string Id { get; set; }
        public string TextSend { get; set; }
        public string To { get; set; }
        public int IsProcessing { get; set; }
        public int Version { get; set; }
        public string Status { get; set; }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
    }
}