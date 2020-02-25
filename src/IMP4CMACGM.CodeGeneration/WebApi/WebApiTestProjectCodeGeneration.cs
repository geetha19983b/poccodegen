using IMP4CMACGM.Core.Common;
using IMP4CMACGM.Core.Models;
using IMP4CMACGM.Core.WebApi.Models;
using System.IO;
using System.Threading.Tasks;

namespace IMP4CMACGM.CodeGeneration.WebApi
{
    public class WebApiTestProjectCodeGeneration
    {
        public async Task GenerateTestProjectCode(CodeGenConfig config, WebApiProjectStructure webApiProjectStructure)
        {

            string apiSolutionPath = config.outputFolderPath + "\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + ".sln";

            DirectoryInfo rootTestDirectory = new DirectoryInfo(config.outputFolderPath);
            string[] folders = System.IO.Directory.GetDirectories(config.outputFolderPath);

            if (folders != null && folders.Length > 0)
            {
                if (Directory.Exists(config.outputFolderPath))
                {
                    rootTestDirectory = Directory.CreateDirectory(config.outputFolderPath + "\\" + webApiProjectStructure.TestProject.RootTestDirectory);
                }

                var apiTestProjectDirectory = rootTestDirectory + "\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "." + webApiProjectStructure.TestProject.TestProjectNameSuffix;

                if (Directory.Exists(rootTestDirectory.FullName))
                {
                    Directory.CreateDirectory(apiTestProjectDirectory);

                }

                var codeGenerationOptions = new DotNetCoreCommandOptions();
                codeGenerationOptions.ProjectName = config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "." + webApiProjectStructure.TestProject.TestProjectNameSuffix;
                codeGenerationOptions.ProjectType = webApiProjectStructure.TestProject.TestProjectType;
                codeGenerationOptions.OutPutLocation = apiTestProjectDirectory;

                await CodeGenerationCoreOperations.CreateNewDotNetCoreProject(codeGenerationOptions);

                var csprojFilePath = codeGenerationOptions.OutPutLocation + "\\" + codeGenerationOptions.ProjectName + ".csproj";
                while (!File.Exists(csprojFilePath))
                {
                    // Console.WriteLine("Not Present" + csprojFilePath);
                }
                await CodeGenerationCoreOperations.AddProjectToSoluion(apiSolutionPath, csprojFilePath);

            }
        }
    }
}
