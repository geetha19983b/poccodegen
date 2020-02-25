using System.Threading.Tasks;

namespace IMP4CMACGM.Core.Interfaces
{
    public interface ICommandLineProcessor
    {
        Task<int> Process(string[] args);
    }
}
