namespace Semantics
{
    public class MMethod
    {
        public string Name { get; }
        public string Description { get; }
        public MClass Class { get; internal set; }

        public MMethod(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}