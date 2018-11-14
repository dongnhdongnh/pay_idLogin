using Newtonsoft.Json;
using VakaxaIDServer.Commons.Helpers;

namespace VakaxaIDServer.Models.Domains
{
    public class ReturnObject
    {
        [JsonProperty(Order = 1)]
        public string Status { get; set; }

        [JsonProperty(Order = 2)]
        public string Message { get; set; }

        [JsonProperty(Order = 3)]
        public string Data { get; set; }

//        public string ToJson() =>
//            JsonHelper.SerializeObject(this, JsonHelper.ConvertSettings);

        public static ReturnObject FromJson(string json) =>
            JsonHelper.DeserializeObject<ReturnObject>(json, JsonHelper.ConvertSettings);
    }

    public class ReturnDataObject : ReturnObject
    {
        [JsonProperty(Order = 4)]
        public new object Data { get; set; }
    }
}