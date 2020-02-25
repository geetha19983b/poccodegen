using Newtonsoft.Json;

namespace IMP4CMACGM.Core.WebApi.Models
{
    public class WebApiProjectInformation
    {
        [JsonProperty("rootWebAPIProjectNameSuffix")]
        public string RootWebAPIProjectNameSuffix { get; set; }

        [JsonProperty("apiVersion")]
        public string ApiVersion { get; set; }

        [JsonProperty("dotnetCoreVersion")]
        public string DotnetCoreVersion { get; set; }
    }
}
