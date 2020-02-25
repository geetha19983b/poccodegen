using IMP4CMACGM.CodeGeneration.CSharp.Common;
using IMP4CMACGM.Core.Common;
using IMP4CMACGM.Core.Enums;
using IMP4CMACGM.Core.Models;
using IMP4CMACGM.Core.WebApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace IMP4CMACGM.CodeGeneration.WebApi
{
    public static class MustacheCodeGeneration
    {
        /// <summary>
        /// Generate and customize API code based on Mustache template
        /// </summary>
        /// <param name="config">Configuration values to customize API Code</param>
        /// <param name="webApiProjectStructure">API Structure and dependencies</param>
        public static async Task CustomizeAPICode(CodeGenConfig config, WebApiProjectStructure webApiProjectStructure)
        {
            //Generate Startup.cs file

            await GenerateStartupCode(config, webApiProjectStructure);

            //Generate Docker file
            await GenerateDockerfileCode(config, webApiProjectStructure);

            //Generate Deployment Manifest File
            await GenerateDeploymentManifestCode(config);

            //Generate Pipeline code
            await GeneratePipelineCode(config);

            //Generate Jenkins file
            await GenerateJenkinsfileCode(config);

            //Generate UpdateAssembly file
            await GenerateUpdateAssemblyCode(config);

            //Generate AppSettings file
            await GenerateAppSettings(config, webApiProjectStructure);

            await GenerateGitIgnoreCode(config);
        }
        private static async Task GenerateStartupCode(CodeGenConfig config, WebApiProjectStructure webApiProjectStructure)
        {
            string csprojAPIPath = config.outputFolderPath + "\\src\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + ".csproj";
            string fileName = "Startup";
            string fileExtension = "cs";

            string startupFilePath = config.outputFolderPath + "\\src\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "\\" + fileName + "." + fileExtension;

            dynamic startupConfig = new JObject();
            if (config.UseCircuitbreaker == true)
            {
                startupConfig.isPolly = true;

                //Add Polly Package to Web API Project
                await CodeGenerationCoreOperations.AddPackageToProject(csprojAPIPath, webApiProjectStructure.PollyWebApiPackage.PackageName, webApiProjectStructure.PollyWebApiPackage.Version);
            }

            if (config.UseLogging == true)
            {
                startupConfig.isSteeltoe = true;

                //Add Steeltoe Package to Web API Project
                await CodeGenerationCoreOperations.AddPackageToProject(csprojAPIPath, webApiProjectStructure.SteeltoeWebApiPackage.PackageName, webApiProjectStructure.SteeltoeWebApiPackage.Version);
            }

            if (config.Database == EnumDatabase.MongoDB)
            {
                startupConfig.isMongo = true;
                startupConfig.mongoConnectionName = config.PackageName + "MongoConnectionString";
                //Add Mongo Package
                await CodeGenerationCoreOperations.AddPackageToProject(csprojAPIPath, webApiProjectStructure.MongoDBDriver.PackageName, webApiProjectStructure.MongoDBDriver.Version);
                await CodeGenerationCoreOperations.AddPackageToProject(csprojAPIPath, webApiProjectStructure.MongoDBDriverCore.PackageName, webApiProjectStructure.MongoDBDriverCore.Version);
                await CodeGenerationCoreOperations.AddPackageToProject(csprojAPIPath, webApiProjectStructure.MongoBSON.PackageName, webApiProjectStructure.MongoBSON.Version);
                await CodeGenerationCoreOperations.AddPackageToProject(csprojAPIPath, webApiProjectStructure.MongoHealthCheck.PackageName, webApiProjectStructure.MongoHealthCheck.Version);

            }
            startupConfig.packageName = config.PackageName;
            startupConfig.apiTitle = config.PackageName + " " + webApiProjectStructure.RootWebAPIProjectNameSuffix;
            startupConfig.apiVersion = webApiProjectStructure.ApiVersion;


            await MustacheParser.GenerateCodeFile(fileName, startupFilePath, startupConfig);

        }

        private static async Task GenerateDockerfileCode(CodeGenConfig config, WebApiProjectStructure webApiProjectStructure)
        {
            string fileName = "Dockerfile";

            string dockerFilePath = config.outputFolderPath + "\\src\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "\\" + fileName;

            dynamic dockerConfig = new JObject();

            dockerConfig.packageName = config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix;
            dockerConfig.donotcoreVersion = webApiProjectStructure.DotnetCoreVersion;


            await MustacheParser.GenerateCodeFile(fileName, dockerFilePath, dockerConfig);

        }

        private static async Task GenerateDeploymentManifestCode(CodeGenConfig config)
        {
            string fileName = "deployit-manifest";
            string fileExtension = "xml";

            string deploymentManifestFilePath = config.outputFolderPath + "\\" + fileName + "." + fileExtension;

            dynamic deploymentManifestConfig = new JObject();

            deploymentManifestConfig.packageName = config.PackageName;
            deploymentManifestConfig.packageNameLowerCase = config.PackageName.ToLower();
            deploymentManifestConfig.websiteName = "{{NET_ECOMMERCE-API-" + config.PackageName.ToUpper() + "_WEBSITE_NAME}}";
            deploymentManifestConfig.websitePhysicalPath = "{{NET_ECOMMERCE-API-" + config.PackageName.ToUpper() + "_WEBSITE_PHYSICALPATH}}";


            await MustacheParser.GenerateCodeFile(fileName, deploymentManifestFilePath, deploymentManifestConfig);

        }

        private static async Task GeneratePipelineCode(CodeGenConfig config)
        {

            string fileName = "pipeline";
            string fileExtension = "yml";

            string pipelineFilePath = config.outputFolderPath + "\\" + fileName + "." + fileExtension;

            dynamic pipelineConfig = new JObject();

            pipelineConfig.packageName = config.PackageName;

            await MustacheParser.GenerateCodeFile(fileName, pipelineFilePath, pipelineConfig);

        }

        private static async Task GenerateJenkinsfileCode(CodeGenConfig config)
        {
            string fileName = "Jenkinsfile";

            string jenkinsFilePath = config.outputFolderPath + "\\" + fileName;

            dynamic jenkinsfileConfig = new JObject();

            jenkinsfileConfig.packageName = config.PackageName;

            await MustacheParser.GenerateCodeFile(fileName, jenkinsFilePath, jenkinsfileConfig);

        }
        private static async Task GenerateUpdateAssemblyCode(CodeGenConfig config)
        {
            string fileName = "UpdateAssembly";
            string fileExtension = "ps1";

            string updateAssemblyFilePath = config.outputFolderPath + "\\" + fileName + "." + fileExtension;

            dynamic updateAssemblyConfig = new JObject();

            updateAssemblyConfig.packageName = config.PackageName;

            await MustacheParser.GenerateCodeFile(fileName, updateAssemblyFilePath, updateAssemblyConfig);

        }

        private static async Task GenerateGitIgnoreCode(CodeGenConfig config)
        {
            string fileName = "";
            string fileExtension = "gitignore";

            string updateAssemblyFilePath = config.outputFolderPath + "\\" + fileName + "." + fileExtension;

            dynamic updateGitIgnore = new JObject();

            updateGitIgnore.packageName = config.PackageName;

            await MustacheParser.GenerateCodeFile("gitignore", updateAssemblyFilePath, updateGitIgnore);

        }
        private static async Task GenerateAppSettings(CodeGenConfig config, WebApiProjectStructure webApiProjectStructure)
        {
            string fileName = "appsettings";
            string fileExtension = "json";

            string appSettingsPath = config.outputFolderPath + "\\src\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "\\" + fileName + "." + fileExtension;

            dynamic appSettingsConfig = new JObject();

            if (config.Database == EnumDatabase.MongoDB)
            {
                appSettingsConfig.isMongo = true;
                appSettingsConfig.mongoConnectionName = config.PackageName + "MongoConnectionString";
            }

            if (config.Database == EnumDatabase.Oracle)
            {
                appSettingsConfig.isOracle = true;
                appSettingsConfig.oracleConnectionName = config.PackageName + "OracleConnectionString";
            }
            if (config.Messaging.Contains("kafka"))
            {
                appSettingsConfig.isKafka = true;
            }
            else
            {
                appSettingsConfig.isKafka = false;
            }
            if (config.Messaging.Contains("ibmmq"))
            {
                appSettingsConfig.isIBMMq = true;
            }
            else
            {
                appSettingsConfig.isIBMMq = false;
            }


            await MustacheParser.GenerateCodeFile(fileName, appSettingsPath, appSettingsConfig);

        }
    }
}
