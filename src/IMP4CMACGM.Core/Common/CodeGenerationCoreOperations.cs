using IMP4CMACGM.Core.Models;
using System.Threading.Tasks;
using static IMP4CMACGM.Core.Common.ProcessAsyncHelper;

namespace IMP4CMACGM.Core.Common
{
    public static class CodeGenerationCoreOperations
    {
        public static string GenerateCommandForSwaggerCodeGen(CodeGenConfig config)
        {
            string command = string.Empty;
            if (!string.IsNullOrEmpty(config.ExecutableJarFilePath))
            {
                command += "-jar " + config.ExecutableJarFilePath;
            }

            command += " generate";

            if (!string.IsNullOrEmpty(config.SwaggerFile))
            {
                command += " -i " + config.SwaggerFile;
            }

            if (!string.IsNullOrEmpty(config.TemplateDirectoryPath))
            {
                command += " -t " + config.TemplateDirectoryPath;
            }

            command += " -l aspnet5";

            if (!string.IsNullOrEmpty(config.ConfigForSwaggerCodeGenFilePath))
            {
                command += " -c " + config.ConfigForSwaggerCodeGenFilePath;
            }

            if (!string.IsNullOrEmpty(config.OutPutFolder))
            {
                command += " -o " + config.OutPutFolder;
            }

            return command;
        }

        /// <summary>
        /// To generate DOTNET CLI Command based on the command inputs
        /// </summary>
        /// <param name="config">Class for CLI command Inputs</param>
        /// <returns></returns>
        public static string GenerateCommandForDotNetCoreProjectCreation(DotNetCoreCommandOptions config)
        {
            string command = " new ";
            if (!string.IsNullOrEmpty(config.ProjectType))
            {
                command += config.ProjectType;
            }

            if (!string.IsNullOrEmpty(config.ProjectName))
            {
                command += " -n " + config.ProjectName;
            }

            if (!string.IsNullOrEmpty(config.OutPutLocation))
            {
                command += " -o " + config.OutPutLocation;
            }

            return command;
        }

        public static async Task<ProcessResult> CreateNewDotNetCoreProject(DotNetCoreCommandOptions config)
        {
            return await ProcessAsyncHelper.RunProcessAsync("dotnet",
                CodeGenerationCoreOperations.GenerateCommandForDotNetCoreProjectCreation(config),
                30000).ConfigureAwait(false);
        }
        public static async Task<ProcessResult> AddProjectToSoluion(string solutionPath, string csprojFilePath)
        {
            return await ProcessAsyncHelper.RunProcessAsync("dotnet",
                 $" sln {solutionPath} add {csprojFilePath}",
                30000).ConfigureAwait(false);

        }
        public static async Task<ProcessResult> AddPackageToProject(string csprojFilePath, string packageName, string version = "")
        {
            string arguments = string.Empty;
            if (string.IsNullOrEmpty(version))
            {
                arguments = $" add {csprojFilePath} package {packageName} -n";
            }
            else
            {
                arguments = $" add {csprojFilePath} package {packageName} -v {version} -n";
            }

            return await ProcessAsyncHelper.RunProcessAsync("dotnet",
                 arguments,
                30000).ConfigureAwait(false);
        }

        public static async Task<ProcessResult> AddProjectReference(string targetProject, string referenceProject)
        {
            return await ProcessAsyncHelper.RunProcessAsync("dotnet",
                 $" add {targetProject} reference {referenceProject}",
                30000).ConfigureAwait(false);
        }
    }
}
