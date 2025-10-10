using OptimaJet.Workflow.Core;
using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;

namespace WorkflowLib.Activities
{
    public sealed class AssignInspectorActivity : ActivityBase
    {
        public AssignInspectorActivity()
        {
            Type = "AssignInspectorActivity";
            Title = "Assign Inspector";
            Description = "Assign an inspector to the current process.";

            // Template file names (optional, used in Workflow Designer)
            Template = "assignInspectorActivity";
            SVGTemplate = "assignInspectorActivity";
            // Define dropdown parameter (frontend handles the population)
            Parameters = new List<CodeActionParameterDefinition>
            {
                new CodeActionParameterDefinition
                {
                    Name       = "AssignedInspectorId",
                    Title      = "Inspector",
                    Type       = ParameterType.Dropdown,
                    IsRequired = true
                },
                new CodeActionParameterDefinition
                {
                    Name       = "AssignedInspectorName",
                    Title      = "Inspector Name",
                    Type       = ParameterType.Text,
                    IsRequired = false
                }
            };
        }

        /// <summary>
        /// Called before the activity is executed.
        /// Here, we populate the dropdown list with static values.
        /// </summary>
        public override Task PreExecutionAsync(
            WorkflowRuntime runtime,
            ProcessInstance processInstance,
            Dictionary<string, string> parameters,
            CancellationToken token)
        {
            Console.WriteLine("test");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called during execution of the activity.
        /// Reads the selected inspector and stores it in the workflow context.
        /// </summary>
        public override async Task ExecutionAsync(
            WorkflowRuntime runtime,
            ProcessInstance processInstance,
            Dictionary<string, string> parameters,
            CancellationToken token)
        {
            parameters.TryGetValue("AssignedInspectorId", out string? inspectorId);
            parameters.TryGetValue("AssignedInspectorName", out string? inspectorName);

            if (string.IsNullOrWhiteSpace(inspectorId))
            {
                inspectorId = "No inspector selected";
                inspectorName = string.Empty;
            }

            // Save values to workflow context
            await processInstance.SetParameterAsync("AssignedInspectorId", inspectorId, ParameterPurpose.Persistence);
            await processInstance.SetParameterAsync("AssignedInspectorName", inspectorName ?? string.Empty, ParameterPurpose.Persistence);
        }
    }
}
