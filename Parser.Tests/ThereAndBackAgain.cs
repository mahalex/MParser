using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Parser.Tests
{
    public class ThereAndBackAgain
    {
        static ThereAndBackAgain()
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

        [Theory]
        [MemberData(nameof(FilesData))]
        public void TestFile(string fileName)
        {
            var text = File.ReadAllText(fileName);
            var window = new TextWindowWithNull(text, fileName);
            var parser = CreateParser(window);
            var tree = parser.Parse();
            var actualText = tree.Root.FullText;
            var actualWidth = tree.Root.FullWidth;
            Assert.Equal(text, actualText);
            Assert.Equal(text.Length, actualWidth);
        }

        [Theory]
        [MemberData(nameof(FilesData))]
        public void TestLeadingAndTrailingTrivia(string fileName)
        {
            var text = File.ReadAllText(fileName);
            var window = new TextWindowWithNull(text, fileName);
            var parser = CreateParser(window);
            var tree = parser.Parse();

            var sb = new StringBuilder();
            var maybeLeadingTrivia = tree.Root.LeadingTrivia;
            var maybeTrailingTrivia = tree.Root.TrailingTrivia;
            if (maybeLeadingTrivia is SyntaxTriviaList leadingTrivia)
            {
                sb.Append(leadingTrivia.FullText);
            }

            sb.Append(tree.Root.Text);

            if (maybeTrailingTrivia is SyntaxTriviaList trailingTrivia)
            {
                sb.Append(trailingTrivia.FullText);
            }

            var actualText = sb.ToString();
            Assert.Equal(text, actualText);
        }

        public static IEnumerable<object[]> FilesData()
        {
            return Files().Select(fileName => new object[] { fileName });
        }

        public static IEnumerable<string> Files()
        {
            return Files(BaseDirectory);
        }
        
        public static IEnumerable<string> Files(string path)
        {
            var fileNames = Directory.GetFiles(path, "*.m");
            foreach (var fileName in fileNames)
            {
                var relativePath = Path.GetRelativePath(BaseDirectory, fileName);
                if (SkipFiles.Contains(relativePath))
                {
                    continue;
                }

                yield return fileName;
            }

            var subDirectories = Directory.GetDirectories(path);
            foreach (var subDirectory in subDirectories)
            {
                foreach (var fileName in Files(subDirectory))
                {
                    yield return fileName;
                }
            }
        }
    }
}
