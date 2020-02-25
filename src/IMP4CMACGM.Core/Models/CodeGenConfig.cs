using IMP4CMACGM.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IMP4CMACGM.Core.Models
{
    public class CodeGenConfig
    {

        public string ConfigFile { get; set; }

        [JsonProperty("packageName")]
        public string PackageName { get; set; }

        [JsonProperty("packageVersion")]
        public string PackageVersion { get; set; } = "1.0.0";

        [JsonProperty("sourceFolder")]
        public string SourceFolder { get; set; } = "src";

        [JsonProperty("targetFramework")]
        public string TargetFramework { get; set; } = "netcoreapp3.0";

        [JsonProperty("sortParamsByRequiredFlag")]
        public bool SortParamsByRequiredFlag { get; set; } = true;

        [JsonProperty("useDateTimeOffset")]
        public bool UseDateTimeOffset { get; set; } = true;

        [JsonProperty("useCollection")]
        public bool UseCollection { get; set; } = false;

        [JsonProperty("returnICollection")]
        public bool ReturnICollection { get; set; } = false;

        [JsonProperty("optionalMethodArgument")]
        public bool OptionalMethodArgument { get; set; } = true;

        [JsonProperty("optionalAssemblyInfo")]
        public bool OptionalAssemblyInfo { get; set; } = true;

        [JsonProperty("optionalProjectFile")]
        public bool OptionalProjectFile { get; set; } = true;

        [JsonProperty("optionalEmitDefaultValues")]
        public bool OptionalEmitDefaultValues { get; set; } = true;

        [JsonProperty("netCoreProjectFile")]
        public bool NetCoreProjectFile { get; set; } = true;

        [JsonProperty("outPutFolder")]
        public string OutPutFolder { get; set; } = null;

        [JsonProperty("executableJarFilePath")]
        public string ExecutableJarFilePath { get; set; } = null;

        [JsonProperty("templateDirectoryPath")]
        public string TemplateDirectoryPath { get; set; } = null;

        [JsonProperty("configForSwaggerCodeGenFilePath")]
        public string ConfigForSwaggerCodeGenFilePath { get; set; } = null;

        [JsonProperty("database")]
        public EnumDatabase Database { get; set; } = EnumDatabase.MongoDB;

        [JsonProperty("useCircuitbreaker")]
        public bool? UseCircuitbreaker { get; set; } = false;

        [JsonProperty("useMessaging")]
        public bool? UseMessaging { get; set; } = false;

        [JsonProperty("Messaging")]
        public List<string> Messaging { get; set; }

        [JsonProperty("TemplateType")]
        public string TemplateType { get; set; }

        [JsonProperty("useCaching")]
        public bool? UseCaching { get; set; } = false;

        [JsonProperty("swaggerFile")]
        public string SwaggerFile { get; set; }

        [JsonProperty("useLogging")]
        public bool? UseLogging { get; set; } = true;

        [JsonProperty("generateDeploymentArtifacts")]
        public bool GenerateDeploymentArtifacts { get; set; } = true;

        [JsonProperty("gitlabProject")]
        public Uri GitlabProject { get; set; }


        [JsonProperty("namespaceName")]
        public string namespaceName { get; set; }

        [JsonProperty("outputFolderPath")]
        public string outputFolderPath { get; set; }

    }
}
