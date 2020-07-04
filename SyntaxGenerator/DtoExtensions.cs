namespace SyntaxGenerator
{
    public static class DtoExtensions
    {
        public static string GetPrivateFieldName(this FieldDescription field)
        {
            return "_" + field.FieldName;
        }
    }
}