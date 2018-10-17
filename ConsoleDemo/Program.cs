using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Parser;
using ProjectConsole;
using Semantics;

namespace ConsoleDemo
{
    internal class TreeRenderer
    {
        private static void RenderToken(SyntaxToken token, string indent, bool isLast)
        {
            Console.Write(indent + (isLast ? "└── " : "├── "));
            Console.Write($"<{token.Kind}>");
            Console.Write($" {token.Text}");
            Console.WriteLine();
        }

        private static void RenderNode(SyntaxNode node, string indent, bool isLast)
        {
            Console.Write(indent);
            Console.Write(isLast ? "└── " : "├── ");
            Console.Write($"<{node.Kind}>");
            Console.WriteLine();
            var children = node.GetChildNodesAndTokens();
            var last = children.Count - 1;
            indent += isLast ? "    " : "│   ";
            for (var index = 0; index <= last; index++)
            {
                var child = children[index];
                if (child.IsNode)
                {
                    RenderNode(child.AsNode(), indent, index == last);
                } else if (child.IsToken)
                {
                    RenderToken(child.AsToken(), indent, index == last);
                }
            }
        }

        public static void RenderTree(SyntaxTree tree)
        {
            RenderNode(tree.Root, "", true);
        }
    }

    class Program
    {
        private static readonly string BaseDirectory;
        private const string BaseDirectoryMacOs = @"/Applications/MATLAB_R2017b.app/toolbox/matlab/";
        private const string BaseDirectoryWindows = @"D:\Program Files\MATLAB\R2018a\toolbox\matlab\";


        private static MParser CreateParser(ITextWindow window)
        {
            return new MParser(window);
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
        
        private static void ParserDemo()
        {
            Console.WriteLine("Hello World!");
            var text = @"
                function [a, b c] = functionName(d, e, f)
                    a = d + e;
                end
%{
comment
                ";
            var window = new TextWindowWithNull(text, "noname");
            var parser = CreateParser(window);
            var tree = parser.Parse();
            TreeRenderer.RenderTree(tree);
            if (tree.Diagnostics.Any())
            {
                foreach (var diagnostic in tree.Diagnostics)
                {
                    Console.WriteLine($"ERROR: {diagnostic.Message} at position {diagnostic.Span.Start}");
                }
            }
            Console.ReadKey();            
        }

        private static SyntaxTree GetTree(string fileName)
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
            var root = tree.Root;
            var childNodesAndTokens = root.GetChildNodesAndTokens();
            var node = childNodesAndTokens[0].AsNode();
            var classChildNodesAndTokens = node.GetChildNodesAndTokens();
            var c = GetClass.FromTree(root, fileName);
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
            printer.Visit(tree.Root);
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
            printer.Visit(tree.Root);
        }

        public static void Main(string[] args)
        {
            ParserDemo();
            //SemanticsDemo();
            //ContextDemo();
            //DumbPrinterDemo();
            //UsageDemo();
            Console.ReadKey();
        }
    }
}
