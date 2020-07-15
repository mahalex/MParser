namespace Parser.Binding
{
    public class BoundLabel
    {
        public string Name { get; }

        public BoundLabel(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
