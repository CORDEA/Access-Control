using System.Security.AccessControl;

namespace Access_Control
{
    interface IAccessController
    {
        void ChangeOwner();

        void RemoveAccessRules();

        void AddAccessRule();

        AuthorizationRuleCollection GetAccessRules();
    }
}
