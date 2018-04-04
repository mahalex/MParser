using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Lexer;
using Parser;

namespace ProjectConsole
{
    class Program
    {
        private static readonly string BaseDirectory;
        private const string BaseDirectoryMacOs = @"/Applications/MATLAB_R2017b.app/toolbox/matlab/";
        private const string BaseDirectoryWindows = @"C:\Program Files\MATLAB\R2018a\toolbox\matlab\";

        private static HashSet<string> skipFiles = new HashSet<string>
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

        static void ProcessFile(string fileName)
        {
            var text = File.ReadAllText(fileName);
            Console.WriteLine($"Parsing {fileName}...");
            var window = new TextWindowWithNull(text, fileName);
            ILexer<Token> lexer = new MLexer(window, new PureTokenFactory(window));
            var tokens = lexer.ParseAll();
            //AfterFunction(tokens);
            //FirstToken(tokens);
            var parser = new MParser(tokens);
            var tree = parser.Parse();
            var back = string.Join("", tokens.Select(token => token.FullText));
            if (text != back)
            {
                throw new ApplicationException();
            }
        }

        private static readonly int[] firstTokenCount;
        private static readonly int[] afterFunctionCount;

        static Program()
        {
            var maxKind = ((int[]) typeof(TokenKind).GetEnumValues()).Max();
            firstTokenCount = new int[maxKind + 1];
            afterFunctionCount = new int[maxKind + 1];
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

        static void AfterFunction(List<Token> tokens)
        {
            for (var i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].PureToken.Kind == TokenKind.Identifier &&
                    tokens[i].PureToken.LiteralText == "function")
                {
                    var nextKind = tokens[i + 1].PureToken.Kind;
                    afterFunctionCount[(int) nextKind]++;
                    if (nextKind != TokenKind.Identifier && nextKind != TokenKind.OpeningSquareBracket)
                    {
                        Console.WriteLine("===EXAMPLE===");
                        Console.WriteLine($"{tokens[i]}{tokens[i+1]}");
                    }
                }
            }
        }

        static void FirstToken(List<Token> tokens)
        {
            var firstKind = tokens[0].PureToken.Kind;
            firstTokenCount[(int) firstKind]++;
        }

        static void AfterFunctionFinish()
        {
            for (var i = 0; i < afterFunctionCount.Length; i++)
            {
                Console.WriteLine($"{(TokenKind)i}: {afterFunctionCount[i]}.");
            }
        }

        static void FirstTokenFinish()
        {
            for (var i = 0; i < firstTokenCount.Length; i++)
            {
                if (firstTokenCount[i] != 0)
                {
                    Console.WriteLine($"{(TokenKind) i}: {firstTokenCount[i]}.");
                }
            }            
        }

        static int ProcessDirectory(string directory)
        {
            var counter = 0;
            var files = Directory.GetFiles(directory, "*.m");
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(BaseDirectory, file);
                if (skipFiles.Contains(relativePath))
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

        static void Main(string[] args)
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
    }
}
