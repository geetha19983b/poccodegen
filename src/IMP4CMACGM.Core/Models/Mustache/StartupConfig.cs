using Newtonsoft.Json;

namespace IMP4CMACGM.Core.Models.Mustache
{
    public class StartupConfig
    {
        [JsonProperty("packageName")]
        public string packageName { get; set; }

        [JsonProperty("isPolly")]
        public bool isPolly { get; set; }

        [JsonProperty("isSteeltoe")]
        public bool isSteeltoe { get; set; } = true;

        [JsonProperty("version")]
        public string version { get; set; }

        [JsonProperty("apiTitle")]
        public string apiTitle { get; set; }
    }
}
