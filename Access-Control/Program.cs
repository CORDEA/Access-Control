using System;
using System.IO;
using System.Linq;

namespace Access_Control
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            var path = args[0];
            if (Directory.Exists(path))
            {
                ChangeAccessRules(path);
            }
            else
            {
                if (File.Exists(path))
                {
                    ChangeAccessRule(path);
                }
            }
        }

        private static void ChangeAccessRule(string filePath)
        {
            var fileController = new FileAccessController(filePath);
            fileController.ChangeOwner();
            fileController.RemoveAccessRules();
            fileController.AddAccessRule();
            if (fileController.GetAccessRules().Count > 1)
            {
                Console.Error.WriteLine($"{filePath} access rule setting may not be completed normally.");
            }
        }

        private static void ChangeAccessRules(string dirPath)
        {
            var directoryController = new DirectoryAccessController(dirPath);
            directoryController.ChangeOwner();
            directoryController.RemoveAccessRules();
            directoryController.AddAccessRule();
            if (directoryController.GetAccessRules().Count > 1)
            {
                Console.Error.WriteLine($"{dirPath} access rule setting may not be completed normally.");
            }

            var files = Directory.GetFiles(dirPath);
            foreach (var file in files)
            {
                ChangeAccessRule(file);
            }

            var dirs = Directory.GetDirectories(dirPath);
            foreach (var dir in dirs)
            {
                ChangeAccessRules(dir);
            }
        }
    }
}
