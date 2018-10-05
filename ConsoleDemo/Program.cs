using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Parser;
using ProjectConsole;
using Semantics;

namespace ConsoleDemo
{
    class Program
    {
        private static readonly string BaseDirectory;
        private const string BaseDirectoryMacOs = @"/Applications/MATLAB_R2017b.app/toolbox/matlab/";
        private const string BaseDirectoryWindows = @"D:\Program Files\MATLAB\R2018a\toolbox\matlab\";

        private static readonly HashSet<string> SkipFiles = new HashSet<string>
        {
            @"codetools\private\template.m", // this is a template, so it contains '$' characters.
            @"plottools\+matlab\+graphics\+internal\+propertyinspector\+views\CategoricalHistogramPropertyView.m", // this one contains a 0xA0 character (probably it's 'non-breakable space' in Win-1252).
            @"plottools\+matlab\+graphics\+internal\+propertyinspector\+views\PrimitiveHistogram2PropertyView.m", // same
            @"plottools\+matlab\+graphics\+internal\+propertyinspector\+views\PrimitiveHistogramPropertyView.m", // same
            @"codetools/private/template.m", // this is a template, so it contains '$' characters.
            @"plottools/+matlab/+graphics/+internal/+propertyinspector/+views/CategoricalHistogramPropertyView.m", // this one contains a 0xA0 character (probably it's 'non-breakable space' in Win-1252).
            @"plottools/+matlab/+graphics/+internal/+propertyinspector/+views/PrimitiveHistogram2PropertyView.m", // same
            @"plottools/+matlab/+graphics/+internal/+propertyinspector/+views/PrimitiveHistogramPropertyView.m", // same
        };

        private static MParser CreateParser(ITextWindow window)
        {
            return new MParser(window);
        }

        private static void ProcessFile(string fileName)
        {
            var text = File.ReadAllText(fileName);
            var window = new TextWindowWithNull(text, fileName);
            var parser = CreateParser(window);
            var tree = parser.Parse();
            var actual = tree.FullText;
            if (actual != text)
            {
                throw new ApplicationException();
            }
        }

        static Program()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    BaseDirectory = BaseDirectoryMacOs;
                    break;
                default:
                    BaseDirectory = BaseDirectoryWindows;
                    break;
            }
        }

        private static int ProcessDirectory(string directory)
        {
            var counter = 0;
            var files = Directory.GetFiles(directory, "*.m");
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(BaseDirectory, file);
                if (SkipFiles.Contains(relativePath))
                {
                    continue;
                }
                ProcessFile(file);
                counter++;
            }

            var subDirectories = Directory.GetDirectories(directory);
            foreach (var subDirectory in subDirectories)
            {
                counter += ProcessDirectory(subDirectory);
            }

            return counter;
        }
        
        private static void ParserDemo()
        {
            Console.WriteLine("Hello World!");
            var sw = new Stopwatch();
            sw.Start();
            var processed = ProcessDirectory(BaseDirectory);
            sw.Stop();
            Console.WriteLine($"{processed} files parsed. Elapsed: {sw.Elapsed}.");
            //AfterFunctionFinish();
            //FirstTokenFinish();
            Console.ReadKey();            
        }

        private static FileSyntaxNode GetTree(string fileName)
        {
            var text = File.ReadAllText(fileName);
            var window = new TextWindowWithNull(text, fileName);
            var parser = CreateParser(window);
            var tree = parser.Parse();
            return tree;
        }

        public static void SemanticsDemo()
        {
            var fileName = Path.Combine(
                BaseDirectory,
                "datatypes",
                "@table",
                "table.m");
            var tree = GetTree(fileName);
            var childNodesAndTokens = tree.GetChildNodesAndTokens();
            var node = childNodesAndTokens[0].AsNode();
            var classChildNodesAndTokens = node.GetChildNodesAndTokens();
            var c = GetClass.FromTree(tree, fileName);
            Console.WriteLine(c.Name);
            foreach (var m in c.Methods)
            {
                Console.WriteLine($"* Method {m.Name}");
                if (m.Description != "")
                {
                    Console.WriteLine($"* Description: {m.Description}");
                }
            }
        }

        public static void ContextDemo()
        {
            var context = new Context();
            context.ScanPath(BaseDirectory);
        }

        public static void DumbPrinterDemo()
        {
            var context = new Context();
            context.ScanPath(BaseDirectory);
            var fileName = Path.Combine(
                BaseDirectory,
                "specgraph",
                "heatmap.m");
            var tree = GetTree(fileName);
            var printer = new DumbWalker(context);
            printer.Visit(tree);
        }

        public static void UsageDemo()
        {
            var context = new Context();
            context.ScanPath(BaseDirectory);
            var fileName = Path.Combine(
                BaseDirectory,
                "specgraph",
                "heatmap.m");
            var tree = GetTree(fileName);
            var printer = new UsageGathering(context);
            printer.Visit(tree);
        }

        public static void Main(string[] args)
        {
            //ParserDemo();
            //SemanticsDemo();
            //ContextDemo();
            //DumbPrinterDemo();
            UsageDemo();
            Console.ReadKey();
        }
    }
}
