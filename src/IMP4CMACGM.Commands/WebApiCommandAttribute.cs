using IMP4CMACGM.CodeGeneration;
using IMP4CMACGM.Core.Enums;
using IMP4CMACGM.Core.Models;
using IMP4CMACGM.Generation.WebApi;
using McMaster.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace IMP4CMACGM.Commands
{
    public class WebApiCommandAttribute
    {
        //[Option("-C|--configfile", Description = "Path for Config file with filled values.",ShowInHelpText =true)]
        [Argument(0, "configfile", Description = "Path for Config file with filled values.", ShowInHelpText = true)]
        public string ConfigFile { get; set; }

        [JsonProperty("packageName")]
        [Option("-P|--packagename", Description = "Name of the API package.")]
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
        [Option("-O|--output", Description = "Where to place generated files (current dir by default)")]
        public string OutPutFolder { get; set; }

        [JsonProperty("executableJarFilePath")]
        [Option("-J|--JAR", Description = "Path for Swagger CodeGen CLI Jar File(current dir by default")]
        public string ExecutableJarFilePath { get; set; }

        [JsonProperty("templateDirectoryPath")]
        [Option("-T|--template-dir", Description = "Path for Swagger CodeGen CLI Jar File(current dir by default")]
        public string TemplateDirectoryPath { get; set; }

        [JsonProperty("configForSwaggerCodeGenFilePath")]
        [Option("-C|--configfile-path", Description = "Path for configuration file(current dir by default")]
        public string ConfigForSwaggerCodeGenFilePath { get; set; }

        [JsonProperty("database")]
        [Option("-D|--database", Description = "Name of the API package. MongoDB/Oracle")]
        public EnumDatabase Database { get; set; } = EnumDatabase.MongoDB;

        [JsonProperty("useCircuitbreaker")]
        [Option("-CB|--circuitbreak", Description = "Include policy for Circuit Breaker? True/False")]
        public bool? UseCircuitbreaker { get; set; } = null;

        [JsonProperty("useMessaging")]
        [Option("-M|--messaging", Description = "Include code snippet for Kafka? True/False")]
        public bool? UseMessaging { get; set; } = null;

        [JsonProperty("useCaching")]
        [Option("-CA|--caching", Description = "Include code snippet for caching? True/False")]
        public bool? UseCaching { get; set; } = null;

        [JsonProperty("swaggerFile")]
        [Option("-SW|--swaggerfile", Description = "Path to Swagger yaml file")]
        public string SwaggerFile { get; set; }

        [JsonProperty("generateDeploymentArtifacts")]
        [Option("-DA|--deployartifacts", Description = "Generate deployment artifacts(Deployment,Service yml)? True/False")]
        public bool GenerateDeploymentArtifacts { get; set; } = true;

        [JsonProperty("gitlabProject")]
        [Option("-G|--gitlab", Description = "URL for GitLab project to push the generated code")]
        public Uri GitlabProject { get; set; }

        public void OnExecute()
        {
            
            if (string.IsNullOrEmpty(ConfigFile))
            {
                PackageName = PackageName ?? Prompt.GetString("What is the name of the project?", null, ConsoleColor.Cyan);
                SwaggerFile = SwaggerFile ?? Prompt.GetString("Enter the Path/URL of Swagger file", null, ConsoleColor.Cyan);
                UseCircuitbreaker = UseCircuitbreaker ?? Prompt.GetYesNo("Include Circute Breaker?", false, ConsoleColor.Cyan);
                UseMessaging = UseMessaging ?? Prompt.GetYesNo("Include Messaging?", false, ConsoleColor.Cyan);
                UseCaching = UseCaching ?? Prompt.GetYesNo("Include Caching?", false, ConsoleColor.Cyan);

                var config = new CodeGenConfig
                {
                    PackageName = this.PackageName,
                    SwaggerFile = this.SwaggerFile,
                    ExecutableJarFilePath = this.ExecutableJarFilePath,
                    TemplateDirectoryPath = this.TemplateDirectoryPath,
                    ConfigForSwaggerCodeGenFilePath = this.ConfigForSwaggerCodeGenFilePath,
                    UseCircuitbreaker = this.UseCircuitbreaker,
                    UseMessaging = this.UseMessaging,
                    Database = EnumDatabase.MongoDB,
                    Messaging = new List<string> { "kafka" },
                    UseCaching = this.UseCaching,
                    outputFolderPath = this.OutPutFolder//@"D:\E-Comm\Modernization\CodeGeneratorAPI\CodeGeneratorAPI\Output" //this.OutPutFolder
                };

                WebApiProjectCodeGen codeGen = new WebApiProjectCodeGen();
                var result = codeGen.GenerateProjectCode(config).GetAwaiter().GetResult();

            }

        }
    }
}
