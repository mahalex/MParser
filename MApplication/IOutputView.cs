namespace MApplication
{
    internal interface IOutputView
    {
        int Width { get; }
        int Height { get; }
        void MoveCursorTo(int column, int line);
        void SetStyle(Style style);
        void WriteText(string s);
    }
}
