using Newtonsoft.Json;

namespace VakaxaIDServer.Quickstart.Profile
{
    public class SocialNetwork
    {
        public string Website { get; set; }

        public string Facebook { get; set; }

        public string Twitter { get; set; }

        public string Telegram { get; set; }
        
        public string Skype { get; set; }

        public string LinkedIn { get; set; }

        public static SocialNetwork FromJson(string json) =>
            JsonConvert.DeserializeObject<SocialNetwork>(json);

        public static string ToJson(SocialNetwork self) =>
            JsonConvert.SerializeObject(self);
    }
}