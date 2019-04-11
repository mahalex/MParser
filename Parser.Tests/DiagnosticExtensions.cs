using System.Linq;

namespace Parser.Tests
{
    public static class DiagnosticExtensions
    {
        public static bool IsEquivalentTo(this SyntaxDiagnostic[] actualSequence, params SyntaxDiagnostic[] expectedSequence)
        {
            if (actualSequence.Length != expectedSequence.Length)
            {
                return false;
            }

            var actualSorted = actualSequence.OrderBy(x => x.Position).ToArray();
            var expectedSorted = expectedSequence.OrderBy(x => x.Position).ToArray();
            for (var i = 0; i < expectedSequence.Length; i++)
            {
                var expected = expectedSequence[i];
                var actual = actualSequence[i];
                if (expected.Position != actual.Position)
                {
                    return false;
                }

                if (expected is MissingTokenSyntaxDiagnostic expectedMissingToken)
                {
                    if (!(actual is MissingTokenSyntaxDiagnostic actualMissingToken))
                    {
                        return false;
                    }

                    if (expectedMissingToken.Kind != actualMissingToken.Kind)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static MissingTokenSyntaxDiagnostic MissingToken(int position, TokenKind kind)
        {
            return new MissingTokenSyntaxDiagnostic(position, kind);
        }
    }
}