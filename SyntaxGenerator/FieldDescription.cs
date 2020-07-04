using System.Xml.Serialization;

namespace SyntaxGenerator
{

    public class FieldDescription
    {
        [XmlAttribute("Type")]
        public string FieldType { get; set; }

        [XmlAttribute("Name")]
        public string FieldName { get; set; }

        [XmlAttribute("Nullable")]
        public bool FieldIsNullable { get; set; }
    }
}