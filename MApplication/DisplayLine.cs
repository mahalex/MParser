using System.Collections.Immutable;
using System.Linq;

namespace MApplication
{
    internal class DisplayLine
    {
        public DisplayLine(ImmutableList<DisplayLineChunk> chunks)
        {
            Chunks = chunks;
            Width = chunks.Sum(c => c.Width);
        }

        public ImmutableList<DisplayLineChunk> Chunks { get; }

        public int Width { get; }
    }
}
