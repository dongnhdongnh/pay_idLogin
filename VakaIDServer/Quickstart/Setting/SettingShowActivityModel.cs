using Newtonsoft.Json;

namespace VakaxaIDServer.Quickstart.Setting
{
    public class SettingShowActivityModel
    {
        public bool UpdatePhone { get; set; }
        public bool UpdatePassword { get; set; }
        public bool DeactivateAccount { get; set; }
        public bool TwoFactor { get; set; }
        public bool Avatar { get; set; }
        public bool Login { get; set; }
        public bool Logout { get; set; }
       
        public static SettingShowActivityModel FromJson(string json) =>
            JsonConvert.DeserializeObject<SettingShowActivityModel>(json);

        public static string ToJson(SettingShowActivityModel self) =>
            JsonConvert.SerializeObject(self);
    }
}