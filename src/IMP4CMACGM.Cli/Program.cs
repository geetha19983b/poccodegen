using IMP4CMACGM.Commands;
using IMP4CMACGM.Core.Interfaces;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace IMP4CMACGM.Cli
{

    class Program
    {

        private static IServiceProvider _serviceProvider;
        static async Task<int> Main(string[] args)
        {
            RegisterServices();
            var service = _serviceProvider.GetService<ICommandLineProcessor>();
            int value = await service.Process(args);
            DisposeServices();
            return value;
        }
        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddScoped<ICommandLineProcessor, CommandLineProcessor>();
            collection.AddSingleton<IConsole>(PhysicalConsole.Singleton);
            // ...
            // Add other services
            // ...
            _serviceProvider = collection.BuildServiceProvider();
        }
        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }

    }
}
