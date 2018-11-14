using System.Collections.Generic;
using Newtonsoft.Json;

namespace VakaxaIDServer.Models
{
    public class CountryModel
    {
        public string Name { get; set; }
        public string CallingCode { get; set; }
        public string Code { get; set; }

        public static List<CountryModel> FromJson(string json) =>
            JsonConvert.DeserializeObject<List<CountryModel>>(json);

        public static string ToJson(List<CountryModel> self) => JsonConvert.SerializeObject(self);
    }
}