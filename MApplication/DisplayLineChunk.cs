using System;

namespace MApplication
{
    internal class DisplayLineChunk
    {
        public DisplayLineChunk(ReadOnlyMemory<char> text, Style style)
        {
            Text = text;
            Style = style;
        }

        public ReadOnlyMemory<char> Text { get; }

        public Style Style { get; }

        public int Width => Text.Length;
    }
}
