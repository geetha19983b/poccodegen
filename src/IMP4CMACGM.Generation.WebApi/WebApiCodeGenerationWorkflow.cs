using System;
using System.Collections.Generic;
using System.Text;
using Elsa.Services;
using Elsa.Services.Models;
using Microsoft.Extensions.DependencyInjection;

namespace IMP4CMACGM.Generation.WebApi
{
    public class WebApiCodeGenerationWorkflow : IWorkflow
    {
      
        public void CodeGenerationWorkFlow()
        {
            var services = new ServiceCollection()
               .AddElsaCore()
               //.AddActivity<PackageName>()
               //.AddActivity<CircutBreaker>()
               .BuildServiceProvider();

            // Create a workflow.
            var workflowFactory = services.GetRequiredService<IWorkflowFactory>();
          //  var workflow = workflowFactory.CreateWorkflow<WebApiWorkflow>();

            // Start the workflow.
            var invoker = services.GetService<IWorkflowInvoker>();
            invoker.StartAsync(workflow).GetAwaiter().GetResult();
        }
       
    }
}
