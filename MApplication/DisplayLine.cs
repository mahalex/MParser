using System.Collections.Immutable;

namespace MApplication
{
    internal class DisplayLine
    {
        public ImmutableList<DisplayLineChunk> Chunks { get; }

        public DisplayLine(ImmutableList<DisplayLineChunk> chunks)
        {
            Chunks = chunks;
        }
    }
}
