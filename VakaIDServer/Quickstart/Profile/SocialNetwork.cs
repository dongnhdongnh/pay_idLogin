using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace VakaxaIDServer.Quickstart.Profile
{
    public class SocialNetwork
    {
        [MaxLength(255)]
        public string Website { get; set; }
        [MaxLength(255)]
        public string Facebook { get; set; }
        [MaxLength(255)]
        public string Twitter { get; set; }
        [MaxLength(255)]
        public string Telegram { get; set; }
        [MaxLength(255)]
        public string Skype { get; set; }
        [MaxLength(255)]
        public string LinkedIn { get; set; }

        public static SocialNetwork FromJson(string json) =>
            JsonConvert.DeserializeObject<SocialNetwork>(json);

        public static string ToJson(SocialNetwork self) =>
            JsonConvert.SerializeObject(self);
    }
}