using IMP4CMACGM.Core.Interfaces;
using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;

namespace IMP4CMACGM.Commands
{
    public class CommandLineProcessor : ICommandLineProcessor
    {
        public Task<int> Process(string[] args)
        {
            var app = new CommandLineApplication<WebApiCommandAttribute>();
            app.Conventions
                .UseDefaultConventions();
            return app.ExecuteAsync(args);

        }

    }
}
