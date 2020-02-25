using IMP4CMACGM.Core.Common;
using IMP4CMACGM.Core.Interfaces;
using IMP4CMACGM.Core.Models;
using IMP4CMACGM.Core.WebApi.Models;
using System.IO;
using System.Threading.Tasks;
using static IMP4CMACGM.Core.Common.ProcessAsyncHelper;

namespace IMP4CMACGM.CodeGeneration.WebApi
{
    public class WebApiServerCodegen : IAspNetCoreServerCodegen
    {
        const string SWAGGER_CODEGEN_JAR = @"SwaggerCodeGen\swagger-codegen-cli-3.0.16.jar";  //To be moved to config
        public WebApiServerCodegen()
        {

        }
        public async Task<ProcessResult> GenerateServerCode(string rootWebAPIProjectNameSuffix, CodeGenConfig config)
        {
            var jsonfile = config.SwaggerFile;
            var strJarFilePath = Directory.GetCurrentDirectory() + "\\" + SWAGGER_CODEGEN_JAR;
            var mustacheTemplatePath = Directory.GetCurrentDirectory() + @"\SwaggerCodeGen\AspDotNetCoreMustacheTemplate";
            var rootApiProjectName = config.PackageName + "." + rootWebAPIProjectNameSuffix;
            return await ProcessAsyncHelper.RunProcessAsync("java.exe",
                 $"-jar {strJarFilePath} generate -i {jsonfile} -t {mustacheTemplatePath} -DpackageName={rootApiProjectName} -l aspnetcore  -o {config.outputFolderPath}",
                30000).ConfigureAwait(false);

        }

        public async Task AddProjectReferenceForWebApiProjects(CodeGenConfig config, WebApiProjectStructure webApiProjectStructure)
        {
            //Add Project Reference for API project

            string csprojAPIPath = config.outputFolderPath + "\\src\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + ".csproj";
            var csprojAPITestPath = config.outputFolderPath + "\\" + webApiProjectStructure.TestProject.RootTestDirectory + "\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "." + webApiProjectStructure.TestProject.TestProjectNameSuffix + "\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "." + webApiProjectStructure.TestProject.TestProjectNameSuffix + ".csproj";

            while (!File.Exists(csprojAPITestPath))
            {
            }
            //Add API Project reference to API Test Project
            await CodeGenerationCoreOperations.AddProjectReference(csprojAPITestPath, csprojAPIPath);

            foreach (var subProject in webApiProjectStructure.SubProjects)
            {
                string csprojSubProjectPath = config.outputFolderPath + "\\src\\" + config.PackageName + "." + subProject.SubProjectPath + "\\" + config.PackageName + "." + subProject.SubProjectName + ".csproj";

                while (!File.Exists(csprojSubProjectPath))
                {
                }
                //Add subProjectReference to main API project
                await CodeGenerationCoreOperations.AddProjectReference(csprojAPIPath, csprojSubProjectPath);

                while (!File.Exists(csprojAPITestPath))
                {
                }
                //Add subProjectReference to API Test project
                await CodeGenerationCoreOperations.AddProjectReference(csprojAPITestPath, csprojSubProjectPath);

                //Add project reference for all depedencies
                if (subProject.ProjectDependencies != null && subProject.ProjectDependencies.Count > 0)
                {
                    foreach (var project in subProject.ProjectDependencies)
                    {
                        var csprojDepedentProject = config.outputFolderPath + "\\src\\" + config.PackageName + "." + project + "\\" + config.PackageName + "." + project + ".csproj";

                        //Add depedent project reference
                        await CodeGenerationCoreOperations.AddProjectReference(csprojSubProjectPath, csprojDepedentProject);
                    }
                }

            }
        }

        public async Task AddPackagesForWebApiProjects(CodeGenConfig config, WebApiProjectStructure webApiProjectStructure)
        {
            //Add Project Reference for API project
            var outPutCodePath = config.outputFolderPath; //@"D:\E-Comm\Modernization\CodeGeneratorAPI\CodeGeneratorAPI\Output";//config.outputFolderPath;
            string csprojAPIPath = outPutCodePath + "\\src\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + ".csproj";
            var csprojAPITestPath = outPutCodePath + "\\" + webApiProjectStructure.TestProject.RootTestDirectory + "\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "." + webApiProjectStructure.TestProject.TestProjectNameSuffix + "\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "." + webApiProjectStructure.TestProject.TestProjectNameSuffix + ".csproj";

            string fileContent = File.ReadAllText(csprojAPIPath);
            if (fileContent.Contains("netcoreapp2.2"))
            {
                fileContent = fileContent.Replace("netcoreapp2.2", "netcoreapp3.1");
                var replaceValue = @"<PackageReference Include=""Microsoft.AspNetCore.App"" />"; //To DO
                fileContent = fileContent.Replace(replaceValue, "");

            }

            await File.WriteAllTextAsync(csprojAPIPath, fileContent);

            //Delete Unwanted Files from Root Folder
            var deleteBuildBatFile = outPutCodePath + "\\" + "build.bat";
            var deleteBuildshFile = outPutCodePath + "\\" + "build.sh";
            if (File.Exists(deleteBuildBatFile))
            {
                File.Delete(deleteBuildBatFile);
            }

            if (File.Exists(deleteBuildshFile))
            {
                File.Delete(deleteBuildshFile);
            }

            //Delete Filter Files
            var filterFolderPath = outPutCodePath + "\\src\\" + config.PackageName + "." + webApiProjectStructure.RootWebAPIProjectNameSuffix + "\\" + "Filters";
            DirectoryInfo filterDir = new DirectoryInfo(filterFolderPath);

            foreach (FileInfo file in filterDir.GetFiles())
            {
                file.Delete();
            }

            while (!File.Exists(csprojAPIPath))
            {
            }

            //Add package to Web API Project
            foreach (var package in webApiProjectStructure.WebApiPackages)
            {
                await CodeGenerationCoreOperations.AddPackageToProject(csprojAPIPath, package.PackageName, package.Version);
            }

            //Add package to Web API Sub Projects
            foreach (var subProject in webApiProjectStructure.SubProjects)
            {
                string csprojSubProjectPath = outPutCodePath + "\\src\\" + config.PackageName + "." + subProject.SubProjectPath + "\\" + config.PackageName + "." + subProject.SubProjectName + ".csproj";

                while (!File.Exists(csprojSubProjectPath))
                {
                }

                foreach (var package in subProject.ProjectPackages)
                {
                    await CodeGenerationCoreOperations.AddPackageToProject(csprojSubProjectPath, package.PackageName, package.Version);
                }

            }

            while (!File.Exists(csprojAPITestPath))
            {
            }
            //Add package to Web API Test Project
            foreach (var package in webApiProjectStructure.TestProject.ProjectPackages)
            {
                await CodeGenerationCoreOperations.AddPackageToProject(csprojAPITestPath, package.PackageName, package.Version);
            }



        }

    }
}

