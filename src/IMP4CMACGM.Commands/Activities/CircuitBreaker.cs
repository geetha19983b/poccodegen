using Elsa.Results;
using Elsa.Services;
using Elsa.Services.Models;
using McMaster.Extensions.CommandLineUtils;
using System;

namespace IMP4CMACGM.Commands.Activities
{
    public class CircutBreaker : Activity
    {
        protected override ActivityExecutionResult OnExecute(WorkflowExecutionContext context)
        {
            Prompt.GetYesNo("Do you want to enable CircuitBreaker?", false, ConsoleColor.Cyan);
            return Done();
        }
    }
}
