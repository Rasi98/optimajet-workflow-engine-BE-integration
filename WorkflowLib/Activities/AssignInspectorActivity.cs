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
        }

        /// <summary>
        /// Called before the activity is executed.
        /// Here, we populate the dropdown list with static values.
        /// </summary>
        public override async Task PreExecutionAsync(
            WorkflowRuntime runtime,
            ProcessInstance processInstance,
            Dictionary<string, string> parameters,
            CancellationToken token)
        {
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
            // Retrieve selected inspector from parameters
            parameters.TryGetValue("SelectedInspector", out string? selectedInspector);

            if (string.IsNullOrWhiteSpace(selectedInspector))
            {
                selectedInspector = "No inspector selected";
            }

            // Save the selected inspector to the workflow process instance
            await processInstance.SetParameterAsync("AssignedInspector",
                selectedInspector,
                ParameterPurpose.Persistence);
        }
    }
}
