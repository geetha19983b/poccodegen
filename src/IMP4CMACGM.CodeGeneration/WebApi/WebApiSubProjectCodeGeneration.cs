using IMP4CMACGM.Core.Common;
using IMP4CMACGM.Core.Models;
using IMP4CMACGM.Core.WebApi.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IMP4CMACGM.CodeGeneration.WebApi
{
    public class WebApiSubProjectCodeGeneration
    {
        /// <summary>
        /// To generate WebAPI Sub Projects and added those projects to root solutions created by Swagger Codegen
        /// </summary>
        /// <param name="webapiName">Package Name from Swagger codegen. example - For ProtoType.Api, webapiName is Prototype </param>
        /// <param name="subProjects"></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public async Task GenerateSubProjectProjectCode(CodeGenConfig config, WebApiProjectStructure webApiProjectStructure)
        {
            string apiRootProjectPath = config.outputFolderPath + "\\" + "src";
            string apiSolutionPath = config.outputFolderPath + "\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + ".sln";

            string[] folders = System.IO.Directory.GetDirectories(apiRootProjectPath);

            if (folders != null && folders.Length > 0)
            {
                foreach (var subProject in webApiProjectStructure.SubProjects)
                {
                    var subProjectDirectoryPath = apiRootProjectPath + "\\" + config.PackageName + "." + subProject.SubProjectPath;
                    if (Directory.Exists(apiRootProjectPath))
                    {
                        Directory.CreateDirectory(subProjectDirectoryPath);
                    }

                    var codeGenerationOptions = new DotNetCoreCommandOptions();
                    codeGenerationOptions.ProjectName = config.PackageName + "." + subProject.SubProjectName;
                    codeGenerationOptions.ProjectType = subProject.SubProjectType;
                    codeGenerationOptions.OutPutLocation = subProjectDirectoryPath;

                    await CodeGenerationCoreOperations.CreateNewDotNetCoreProject(codeGenerationOptions);

                    var csprojFilePath = codeGenerationOptions.OutPutLocation + "\\" + codeGenerationOptions.ProjectName + ".csproj";

                    while (!File.Exists(csprojFilePath))
                    {
                    }

                    await CodeGenerationCoreOperations.AddProjectToSoluion(apiSolutionPath, csprojFilePath);

                }

            }
        }

    }
}
