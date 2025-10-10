using OptimaJet.Workflow.Core.Model;
using OptimaJet.Workflow.Core.Runtime;
using WorkflowLib.Model;

namespace WorkflowLib.Rules
{
    public class SimpleRuleProvider : IWorkflowRuleProvider
    {
        // name of our rule
        private const string RuleCheckRole = "CheckRole";
        private const string RuleInspectorIs3 = "InspectorIs3";

        public List<string> GetRules(string schemeCode, NamesSearchType namesSearchType)
        {
            return new List<string> { RuleCheckRole, RuleInspectorIs3 };
        }

        public bool Check(ProcessInstance processInstance, WorkflowRuntime runtime, string identityId, string ruleName, string parameter)
        {
            switch (ruleName)
            {
                // ✅ Rule 1: Check inspector ID == 3
                case RuleInspectorIs3:
                    {
                        var inspectorIdObj = processInstance.GetParameter("AssignedInspectorId");
                        if (inspectorIdObj == null)
                            return false;

                        var inspectorId = inspectorIdObj.ToString();
                        return inspectorId == "3";
                    }

                // ✅ Rule 2: Check user has required role
                case RuleCheckRole:
                    {
                        if (identityId == null || !Users.UserDict.ContainsKey(identityId))
                            return false;

                        var user = Users.UserDict[identityId];
                        return user.Roles.Contains(parameter);
                    }

                // ❌ Default: Unknown rule
                default:
                    return false;
            }
        }

        public IEnumerable<string> GetIdentities(ProcessInstance processInstance, WorkflowRuntime runtime, string ruleName, string parameter)
        {
            // return all identities (the identity is user name)
            return Users.Data.Select(u => u.Name);
        }

        public async Task<bool> CheckAsync(ProcessInstance processInstance, WorkflowRuntime runtime, string identityId, string ruleName,
            string parameter,
            CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetIdentitiesAsync(ProcessInstance processInstance, WorkflowRuntime runtime, string ruleName,
            string parameter,
            CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public bool IsCheckAsync(string ruleName, string schemeCode)
        {
            // use the Check method instead of CheckAsync
            return false;
        }

        public bool IsGetIdentitiesAsync(string ruleName, string schemeCode)
        {
            // use the GetIdentities method instead of GetIdentitiesAsync
            return false;
        }
    }
}
