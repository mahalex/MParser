using System.Collections.Immutable;

namespace MApplication
{
    internal class DisplayText
    {
        public DisplayText(ImmutableList<DisplayLine> lines)
        {
            Lines = lines;
        }

        public ImmutableList<DisplayLine> Lines { get; }
    }
}
