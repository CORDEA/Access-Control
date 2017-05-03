using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Access_Control
{
    class FileAccessController : IAccessController
    {

        private string FilePath { get; }

        public FileAccessController(string filePath)
        {
            FilePath = filePath;
        }

        public void ChangeOwner()
        {
            var control = File.GetAccessControl(FilePath);
            control.SetOwner(new NTAccount(WindowsIdentity.GetCurrent().Name));
            File.SetAccessControl(FilePath, control);
        }

        public void RemoveAccessRules()
        {
            var control = File.GetAccessControl(FilePath);
            var rules = control.GetAccessRules(true, true, typeof(NTAccount));
            control.SetAccessRuleProtection(true, false);
            foreach (FileSystemAccessRule rule in rules)
            {
                control.RemoveAccessRule(rule);
            }
            File.SetAccessControl(FilePath, control);
        }

        public void AddAccessRule()
        {
            var rule = new FileSystemAccessRule(
                WindowsIdentity.GetCurrent().Name,
                FileSystemRights.FullControl,
                AccessControlType.Allow);

            var control = File.GetAccessControl(FilePath);
            control.SetAccessRule(rule);
            File.SetAccessControl(FilePath, control);
        }


        public AuthorizationRuleCollection GetAccessRules()
        {
            var control = File.GetAccessControl(FilePath);
            return control.GetAccessRules(true, true, typeof(NTAccount));
        }
    }
}
