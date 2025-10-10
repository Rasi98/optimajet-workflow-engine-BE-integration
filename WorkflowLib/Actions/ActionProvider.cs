using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;

namespace WorkflowLib.Actions
{
    public class ActionProvider : IWorkflowActionProvider
    {
        private readonly Dictionary<string, Action<ProcessInstance, WorkflowRuntime, string>> _actions = new();

        private readonly Dictionary<string, Func<ProcessInstance, WorkflowRuntime, string, CancellationToken, Task>>
            _asyncActions = new();

        private readonly Dictionary<string, Func<ProcessInstance, WorkflowRuntime, string, bool>> _conditions = new();

        private readonly Dictionary<string, Func<ProcessInstance, WorkflowRuntime, string, CancellationToken, Task<bool>>>
            _asyncConditions = new();

        public ActionProvider()
        {
            // Register your actions in _actions and _asyncActions dictionaries
            _actions.Add("MyAction", MyAction); // sync
            _asyncActions.Add("MyActionAsync", MyActionAsync); // async

            // Register your conditions in _conditions and _asyncConditions dictionaries
            _conditions.Add("IsInspectorC", IsInspectorC); // sync
            _asyncConditions.Add("MyConditionAsync", MyConditionAsync); // async
        }

        private void MyAction(ProcessInstance processInstance, WorkflowRuntime runtime,
        string actionParameter)
        {
            // Execute your synchronous code here
        }

        private async Task MyActionAsync(ProcessInstance processInstance, WorkflowRuntime runtime,
            string actionParameter, CancellationToken token)
        {
            // Execute your asynchronous code here. You can use await in your code.
        }

        private bool IsInspectorC(ProcessInstance processInstance, WorkflowRuntime runtime, string actionParameter)
        {
            var inspectorId = processInstance.GetParameter("AssignedInspectorId");

            if (inspectorId.Value == null)
                return false;

            return inspectorId.Value.ToString() == "3";
        }

        private async Task<bool> MyConditionAsync(ProcessInstance processInstance, WorkflowRuntime runtime,
            string actionParameter, CancellationToken token)
        {
            // Execute your asynchronous code here. You can use await in your code.
            return false;
        }

        #region Implementation of IWorkflowActionProvider

        public void ExecuteAction(string name, ProcessInstance processInstance, WorkflowRuntime runtime, string actionParameter)
        {
            if (!_actions.ContainsKey(name))
            {
                throw new NotImplementedException($"Action with name {name} isn't implemented");
            }

            _actions[name].Invoke(processInstance, runtime, actionParameter);
        }

        public async Task ExecuteActionAsync(string name, ProcessInstance processInstance, WorkflowRuntime runtime, string actionParameter, CancellationToken token)
        {
            //token.ThrowIfCancellationRequested(); // You can use the transferred token at your discretion
            if (!_asyncActions.ContainsKey(name))
            {
                throw new NotImplementedException($"Async Action with name {name} isn't implemented");
            }

            await _asyncActions[name].Invoke(processInstance, runtime, actionParameter, token);
        }

        public bool ExecuteCondition(string name, ProcessInstance processInstance, WorkflowRuntime runtime, string actionParameter)
        {
            if (_conditions.ContainsKey(name))
            {
                return _conditions[name].Invoke(processInstance, runtime, actionParameter);
            }

            throw new NotImplementedException($"Condition with name {name} isn't implemented");
        }

        public async Task<bool> ExecuteConditionAsync(string name, ProcessInstance processInstance, WorkflowRuntime runtime, string actionParameter, CancellationToken token)
        {
            //token.ThrowIfCancellationRequested(); // You can use the transferred token at your discretion
            if (_asyncConditions.ContainsKey(name))
            {
                return await _asyncConditions[name].Invoke(processInstance, runtime, actionParameter, token);
            }

            throw new NotImplementedException($"Async Condition with name {name} isn't implemented");
        }

        public List<string> GetActions(string schemeCode, NamesSearchType namesSearchType)
        {
            return _actions.Keys.Union(_asyncActions.Keys).ToList();
        }

        public List<string> GetConditions(string schemeCode, NamesSearchType namesSearchType)
        {
            return _conditions.Keys.Union(_asyncConditions.Keys).ToList();
        }

        public bool IsActionAsync(string name, string schemeCode)
        {
            return _asyncActions.ContainsKey(name);
        }

        public bool IsConditionAsync(string name, string schemeCode)
        {
            return _asyncActions.ContainsKey(name);
        }
    }

    #endregion
}