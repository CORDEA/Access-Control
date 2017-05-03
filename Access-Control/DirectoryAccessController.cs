using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Access_Control
{
    class DirectoryAccessController : IAccessController
    {

        private string DirPath { get; }

        public DirectoryAccessController(string dirPath)
        {
            DirPath = dirPath;
        }

        public void ChangeOwner()
        {
            var control = Directory.GetAccessControl(DirPath);
            control.SetOwner(new NTAccount(WindowsIdentity.GetCurrent().Name));
            Directory.SetAccessControl(DirPath, control);
        }

        public void RemoveAccessRules()
        {
            var control = Directory.GetAccessControl(DirPath);
            var rules = control.GetAccessRules(true, true, typeof(NTAccount));
            control.SetAccessRuleProtection(true, false);
            foreach (FileSystemAccessRule rule in rules)
            {
                control.RemoveAccessRule(rule);
            }
            Directory.SetAccessControl(DirPath, control);
        }

        public void AddAccessRule()
        {
            var rule = new FileSystemAccessRule(
                WindowsIdentity.GetCurrent().Name,
                FileSystemRights.FullControl,
                AccessControlType.Allow);

            var control = Directory.GetAccessControl(DirPath);
            control.SetAccessRule(rule);
            Directory.SetAccessControl(DirPath, control);
        }

        public AuthorizationRuleCollection GetAccessRules()
        {
            var control = Directory.GetAccessControl(DirPath);
            return control.GetAccessRules(true, true, typeof(NTAccount));
        }
    }
}
