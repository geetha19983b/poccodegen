using IMP4CMACGM.CodeGeneration.WebApi;
using IMP4CMACGM.Core.Models;
using IMP4CMACGM.Core.WebApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IMP4CMACGM.Generation.WebApi
{
    public class WebApiProjectCodeGen
    {
        const string WEBAPI_Project_Structure_JSON = "WebApiProjectStructure.json";
        public async Task<bool> GenerateProjectCode(CodeGenConfig config)
        {
            bool isCreated = true;

            string webApiFolderStructureJson = Directory.GetCurrentDirectory() + "\\" + WEBAPI_Project_Structure_JSON; //@"D:\E-Comm\Modernization\poc-codegenerator\src\IMP4CMACGM.Generation.WebApi\WebApiProjectStructure.json";
            string rawJson = File.ReadAllText(webApiFolderStructureJson);
            var webApiProjectStructure = JsonConvert.DeserializeObject<WebApiProjectStructure>(rawJson);

            //Generate Web API Project using Swagger CodeGendsad
            WebApiServerCodegen aspNetCoreCodeGen = new WebApiServerCodegen();
            await aspNetCoreCodeGen.GenerateServerCode(webApiProjectStructure.RootWebAPIProjectNameSuffix, config);

            //Generate WebAPI Sub Projects using DOTNET Cli
            var webApiProjectCodeGeneration = new WebApiSubProjectCodeGeneration();
            await webApiProjectCodeGeneration.GenerateSubProjectProjectCode(config, webApiProjectStructure);

            //Generate WebAPI Test Projects using DOTNET Cli
            var apiTestProjectCodeGeneration = new WebApiTestProjectCodeGeneration();
            await apiTestProjectCodeGeneration.GenerateTestProjectCode(config, webApiProjectStructure);

            //Add Project Reference for API project
            await aspNetCoreCodeGen.AddProjectReferenceForWebApiProjects(config, webApiProjectStructure);

            //Add Packages for Web API
            await aspNetCoreCodeGen.AddPackagesForWebApiProjects(config, webApiProjectStructure);

            //Generate and customize API code based on Mustache Template
            await MustacheCodeGeneration.CustomizeAPICode(config, webApiProjectStructure);

            return isCreated;

        }
    }
}
