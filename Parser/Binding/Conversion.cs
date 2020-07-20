namespace Parser.Binding
{
    internal class Conversion
    {
        public static Conversion None = new Conversion(exists: false, isIdentity: false);
        public static Conversion Identity = new Conversion(exists: true, isIdentity: true);
        public static Conversion Implicit = new Conversion(exists: true, isIdentity: false);

        private Conversion(bool exists, bool isIdentity)
        {
            Exists = exists;
            IsIdentity = isIdentity;
        }

        public bool Exists { get; }

        public bool IsIdentity { get; }

        public static Conversion Classify(TypeSymbol from, TypeSymbol to)
        {
            if (from == to)
            {
                return Identity;
            }

            if (to == TypeSymbol.MObject)
            {
                return Implicit;
            }

            return None;
        }
    }
}
