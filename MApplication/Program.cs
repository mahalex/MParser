using Parser;
using System;
using System.IO;

namespace MApplication
{
    internal class FileRenderer
    {
        private readonly IOutputView _outputView;

        public FileRenderer(IOutputView outputView)
        {
            _outputView = outputView;
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

        public void RenderFile(string fileName)
        {
            var tree = GetTree(fileName);
            var text = CodeProcessor.GetText(tree);
            var viewPort = new DisplayTextViewPort(
                text: text,
                width: 80,
                height: 24);

            var needsRedraw = true;
            while (true)
            {
                if (needsRedraw)
                {
                    viewPort.RenderTo(_outputView);
                }

                _outputView.MoveCursorTo(viewPort.CursorRelativeColumn, viewPort.CursorRelativeLine);

                var key = Console.ReadKey(intercept: true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        viewPort = viewPort.CursorLeft(out needsRedraw);
                        break;

                    case ConsoleKey.RightArrow:
                        viewPort = viewPort.CursorRight(out needsRedraw);
                        break;

                    case ConsoleKey.UpArrow:
                        viewPort = viewPort.CursorUp(out needsRedraw);
                        break;

                    case ConsoleKey.DownArrow:
                        viewPort = viewPort.CursorDown(out needsRedraw);
                        break;

                    case ConsoleKey.Home:
                        viewPort = viewPort.CursorHome(out needsRedraw);
                        break;

                    case ConsoleKey.End:
                        viewPort = viewPort.CursorEnd(out needsRedraw);
                        break;
                }
            }
        }

    }

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

        static void Main(string[] args)
        {
            var oldStyle = GetStyle();
            var fileName = Path.Combine(
                BaseDirectory,
                "datatypes",
                "@table",
                "table.m");
            var targetWidth = 80;
            var targetHeight = 24;
            var outputViewPort = new ConsoleWindowView(
                startingColumn: (Console.WindowWidth - targetWidth) / 2,
                startingLine: (Console.WindowHeight - targetHeight) / 2,
                width: targetWidth,
                height: targetHeight);
            var renderer = new FileRenderer(outputViewPort);
            renderer.RenderFile(fileName);

            SetStyle(oldStyle);
        }
    }
}
