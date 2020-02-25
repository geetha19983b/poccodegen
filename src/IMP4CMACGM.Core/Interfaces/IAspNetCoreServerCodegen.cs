using IMP4CMACGM.Core.Models;
using System.Threading.Tasks;
using static IMP4CMACGM.Core.Common.ProcessAsyncHelper;

namespace IMP4CMACGM.Core.Interfaces
{
    public interface IAspNetCoreServerCodegen
    {
        Task<ProcessResult> GenerateServerCode(string rootApiProjectName, CodeGenConfig config);
    }
}
