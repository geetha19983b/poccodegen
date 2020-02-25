using Elsa.Results;
using Elsa.Services;
using Elsa.Services.Models;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace IMP4CMACGM.Commands.Activities
{
    public class PackageName : Activity
    {
        protected override ActivityExecutionResult OnExecute(WorkflowExecutionContext context)
        {
            var receivedInput = Prompt.GetString("What is the name of the project?", null, ConsoleColor.Cyan);
            return Execute(context, receivedInput);
        }
        private ActivityExecutionResult Execute(WorkflowExecutionContext workflowContext, string receivedInput)
        {
                workflowContext.CurrentScope.SetVariable("Name", receivedInput);

            Output.SetVariable("Input", receivedInput);

            return Done();
        }
    }
}
