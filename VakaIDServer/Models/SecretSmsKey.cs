using Newtonsoft.Json;

namespace VakaxaIDServer.Models
{
    public class SecretSmsKey
    {
        public string ChangePassword { get; set; }
        public string ChangePhoneOldPhone { get; set; }
        public string ChangePhoneNewPhone { get; set; }
        public string ChangeTwoFactor { get; set; }
        public string LockAccount { get; set; }
        public string UnlockAccount { get; set; }
        public string AddPhoneNumber { get; set; }
        public string AddLockScreen { get; set; }
        public string Login { get; set; }

        public static SecretSmsKey FromJson(string json) =>
            JsonConvert.DeserializeObject<SecretSmsKey>(json);

        public static string ToJson(SecretSmsKey self) => JsonConvert.SerializeObject(self);
    }
}