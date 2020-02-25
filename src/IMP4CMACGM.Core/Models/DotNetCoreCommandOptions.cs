using Newtonsoft.Json;

namespace IMP4CMACGM.Core.Models
{
    public class DotNetCoreCommandOptions
    {
        [JsonProperty("commandName")]
        public string CommandName { get; set; }

        [JsonProperty("projectType")]
        public string ProjectType { get; set; }

        [JsonProperty("projectName")]
        public string ProjectName { get; set; }

        [JsonProperty("outPutLocation")]
        public string OutPutLocation { get; set; }
    }
}
