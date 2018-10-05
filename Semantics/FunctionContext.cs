namespace Semantics
{
    public class FunctionContext
    {
        public string FileName { get; }

        public FunctionContext(string fileName)
        {
            FileName = fileName;
        }
    }
}