using Parser;
using System;
using System.IO;

namespace MApplication
{
    class Program
    {
        private static readonly string BaseDirectory;
        private const string BaseDirectoryMacOs = @"/Applications/MATLAB_R2017b.app/toolbox/matlab/";
        private const string BaseDirectoryWindows = @"D:\Program Files\MATLAB\R2018a\toolbox\matlab\";

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

        private static MParser CreateParser(ITextWindow window)
        {
            return new MParser(window);
        }

        private static SyntaxTree GetTree(string fileName)
        {
            var text = File.ReadAllText(fileName);
            var window = new TextWindowWithNull(text, fileName);
            var parser = CreateParser(window);
            var tree = parser.Parse();
            return tree;
        }

        private static Style GetStyle()
        {
            return new Style(
                foregroundColor: Console.ForegroundColor,
                backgroundColor: Console.BackgroundColor);
        }

        private static void SetStyle(Style style)
        {
            Console.BackgroundColor = style.BackgroundColor;
            Console.ForegroundColor = style.ForegroundColor;
        }

        private static void PrintChunk(DisplayLineChunk chunk)
        {
            SetStyle(chunk.Style);
            Console.Write(chunk.Text.ToString());
        }

        private static void PrintLine(DisplayLine line)
        {
            foreach (var chunk in line.Chunks)
            {
                PrintChunk(chunk);
            }
        }

        static void RenderFile(string fileName)
        {
            var tree = GetTree(fileName);
            var text = CodeProcessor.GetText(tree);
            var viewPort = new DisplayTextViewPort(
                text: text,
                width: 80,
                height: 24);

            var targetWidth = 80;
            var targetHeight = 24;
            var outputViewPort = new ConsoleWindowView(
                startingColumn: (Console.WindowWidth - targetWidth) / 2,
                startingLine: (Console.WindowHeight - targetHeight) / 2,
                width: targetWidth,
                height: targetHeight);

            while (true)
            {
                viewPort.RenderTo(outputViewPort);
                var key = Console.ReadKey(intercept: true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        viewPort = viewPort.With(startingColumn: Math.Max(viewPort.StartingColumn - 1, 0));
                        break;

                    case ConsoleKey.RightArrow:
                        viewPort = viewPort.With(startingColumn: viewPort.StartingColumn + 1);
                        break;

                    case ConsoleKey.UpArrow:
                        viewPort = viewPort.With(startingLine: Math.Max(viewPort.StartingLine - 1, 0));
                        break;

                    case ConsoleKey.DownArrow:
                        viewPort = viewPort.With(startingLine: viewPort.StartingLine + 1);
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            var oldStyle = GetStyle();
            var fileName = Path.Combine(
                BaseDirectory,
                "datatypes",
                "@table",
                "table.m");
            Console.CursorVisible = false;
            RenderFile(fileName);
            Console.CursorVisible = true;

            //foreach (var line in text.Lines)
            //{
            //    PrintLine(line);
            //    Console.WriteLine();
            //}

            SetStyle(oldStyle);
        }
    }
}
