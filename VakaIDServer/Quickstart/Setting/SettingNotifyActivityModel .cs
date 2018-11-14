using Newtonsoft.Json;

namespace VakaxaIDServer.Quickstart.Setting
{
    public class SettingNotifyActivityModel
    {
        public bool Info { get; set; }
        public bool Login { get; set; }
        public bool Action { get; set; }

        public static SettingNotifyActivityModel FromJson(string json) =>
            JsonConvert.DeserializeObject<SettingNotifyActivityModel>(json);

        public static string ToJson(SettingNotifyActivityModel self) =>
            JsonConvert.SerializeObject(self);
    }
}