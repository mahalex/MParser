using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

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

        private static void ProcessFile(string fileName)
        {
            var text = File.ReadAllText(fileName);
            var window = new TextWindowWithNull(text, fileName);
            var parser = CreateParser(window);
            var tree = parser.Parse();
            var actual = tree.FullText;
            Assert.That(actual == text);
        }

        private static void ProcessDirectory(string directory)
        {
            var files = Directory.GetFiles(directory, "*.m");
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(BaseDirectory, file);
                if (SkipFiles.Contains(relativePath))
                {
                    continue;
                }
                ProcessFile(file);
            }

            var subDirectories = Directory.GetDirectories(directory);
            foreach (var subDirectory in subDirectories)
            {
                ProcessDirectory(subDirectory);
            }
        }

        [Category("Slow")]
        [Test]
        public void TestEverything()
        {
            ProcessDirectory(BaseDirectory);
        }
    }
}
