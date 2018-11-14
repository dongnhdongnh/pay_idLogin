using System.ComponentModel.DataAnnotations.Schema;

namespace VakaxaIDServer.Quickstart.Setting
{
    [Table("SettingActivity")]
    public class SettingActivityModel
    {
        public string Id { get; set; }
        public string NotifyActivity { get; set; }
        public string ShowActivity { get; set; }
        public string UserId { get; set; }
        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }
    }
}