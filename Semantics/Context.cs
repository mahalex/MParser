using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Semantics
{
    public class Context
    {
        internal PackageContext Root { get; }

        public Context()
        {
            Root = new PackageContext(
                null,
                new Dictionary<string, PackageContext>(),
                new Dictionary<string, ClassContext>(),
                new Dictionary<string, FunctionContext>());
        }

        public bool FindFunction(string name)
        {
            return Root.Functions.ContainsKey(name);
        }

        private void ScanFunction(PackageContext context, string fileName)
        {
            var functionName = Path.GetFileNameWithoutExtension(fileName);
            context.Functions[functionName] = new FunctionContext(fileName);
        }

        private void ScanMethod(ClassContext context, string fileName)
        {
            var methodName = Path.GetFileNameWithoutExtension(fileName);
            context.Methods[methodName] = new FunctionContext(fileName);
        }

        private void ScanPrivateMethod(ClassContext context, string fileName)
        {
            var methodName = Path.GetFileNameWithoutExtension(fileName);
            context.PrivateMethods[methodName] = new FunctionContext(fileName);
        }

        private string GetPackageNameFromFolder(string folderName)
        {
            return folderName.StartsWith('+') ? folderName.Substring(1, folderName.Length - 1) : null;
        }
        
        private string GetClassNameFromFolder(string folderName)
        {
            return folderName.StartsWith('@') ? folderName.Substring(1, folderName.Length - 1) : null;
        }

        private void ScanPrivateDirectory(ClassContext currentContext, string directory)
        {
            var files = Directory.GetFiles(directory, "*.m");
            foreach (var fileName in files)
            {
                ScanPrivateMethod(currentContext, fileName);
            }

            var subDirectories = Directory.GetDirectories(directory);
            foreach (var subDirectory in subDirectories)
            {
                Console.WriteLine($"A FOLDER INSIDE A PRIVATE SUBFOLDER WHAT TO DO? {subDirectory}");
            }            

        }

        private void ScanClassDirectory(ClassContext currentContext, string directory)
        {
            var files = Directory.GetFiles(directory, "*.m");
            foreach (var fileName in files)
            {
                ScanMethod(currentContext, fileName);
            }

            var subDirectories = Directory.GetDirectories(directory);
            foreach (var subDirectory in subDirectories)
            {
                var lastName = subDirectory.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Last();
                if (lastName == "private")
                {
                    ScanPrivateDirectory(currentContext, subDirectory);
                    continue;
                }
                var packageName = GetPackageNameFromFolder(lastName);
                if (packageName != null)
                {
                    Console.WriteLine($"A PACKAGE INSIDE A CLASS WHAT TO DO? {subDirectory}");
                    continue;
                }

                var className = GetClassNameFromFolder(lastName);
                if (className != null)
                {
                    currentContext.SubClasses[className] = new ClassContext(className);
                    ScanClassDirectory(currentContext.SubClasses[className], subDirectory);
                    continue;
                }
                ScanClassDirectory(currentContext, subDirectory); // Should this really work?
            }            
        }
        
        private void ScanDirectory(PackageContext currentContext, string directory)
        {
            var files = Directory.GetFiles(directory, "*.m");
            foreach (var fileName in files)
            {
                ScanFunction(currentContext, fileName);
            }

            var subDirectories = Directory.GetDirectories(directory);
            foreach (var subDirectory in subDirectories)
            {
                var lastName = subDirectory.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Last();
                var packageName = GetPackageNameFromFolder(lastName);
                if (packageName != null)
                {
                    currentContext.SubPackages[packageName] = new PackageContext(packageName);
                    ScanDirectory(currentContext.SubPackages[packageName], subDirectory);
                    continue;
                }

                var className = GetClassNameFromFolder(lastName);
                if (className != null)
                {
                    currentContext.Classes[className] = new ClassContext(className);
                    ScanClassDirectory(currentContext.Classes[className], subDirectory);
                    continue;
                }
                ScanDirectory(currentContext, subDirectory);
            }
        }
        
        public void ScanPath(string path)
        {
            ScanDirectory(Root, path);
        }
    }
}