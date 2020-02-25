using IMP4CMACGM.API.Models;
using IMP4CMACGM.Core.Common;
using IMP4CMACGM.Core.Models;
using IMP4CMACGM.Generation.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;


namespace IMP4CMACGM.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebAPIController : ControllerBase
    {
        #region Private members
        private long localDateTime = DateTime.UtcNow.ToFileTime();
        private readonly IHostingEnvironment _hostingEnvironment;
        private GitOperations gitOperations;
        private string gitbranchName = "feature-";
        #endregion

        public WebAPIController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> CreateWebAPI([FromBody]WebAPI webApi)
        {
            if (webApi != null)
            {
                //var outputFolderPath = _hostingEnvironment.ContentRootPath + "\\" + webApi.projectName + localDateTime +"\\";
                var outputFolderPath = @"D:\work\poc\CodeGenOutput" + "\\" + webApi.projectName + localDateTime + "\\";
                while (!Directory.Exists(outputFolderPath))
                {
                    Directory.CreateDirectory(outputFolderPath);
                }
                if (webApi.saveToGit)
                {
                    gitOperations = new GitOperations(webApi.gitUsername, webApi.gitPassword, webApi.gitURL, outputFolderPath);
                    gitOperations.CloneRepo();
                    gitbranchName = gitbranchName + webApi.projectName;
                }

                var config = new CodeGenConfig()
                {
                    Messaging = webApi.messaging,
                    UseCircuitbreaker = webApi.useCircuitBreaker,
                    PackageName = webApi.projectName,
                    namespaceName = webApi.namespaceName,
                    SwaggerFile = webApi.swaggerPath,
                    outputFolderPath = outputFolderPath

                };

                WebApiProjectCodeGen webApiProjectCodeGen = new WebApiProjectCodeGen();
                await webApiProjectCodeGen.GenerateProjectCode(config);

                if (webApi.saveToGit)
                {
                    if (!string.IsNullOrEmpty(webApi.gitUsername) && !string.IsNullOrEmpty(webApi.gitPassword) &&
                        !string.IsNullOrEmpty(webApi.gitURL))
                    {
                        var branches = gitOperations.ListGitBranches(gitbranchName);
                        if (!branches)
                        {
                            gitOperations.CreateBranch(gitbranchName, gitbranchName);
                            gitOperations.PushCommits(gitbranchName, gitbranchName);
                            return Ok(new CreateResponse() { Result = "Project Created. Pushed to Git." });
                        }

                    }
                }
            }
            return Ok(new CreateResponse() { Result = "Project Created. TODO: Download zip file." });
        }
    }
}