using Elsa.Services;
using Elsa.Services.Models;
using IMP4CMACGM.Commands.Activities;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMP4CMACGM.Commands
{
    public class WebApiWorkflow : IWorkflow
    {
        public void Build(IWorkflowBuilder builder)
        {
            builder
                
                .StartWith<PackageName>(name:"Name")
                .Then<CircutBreaker>();
        }
    }
}
