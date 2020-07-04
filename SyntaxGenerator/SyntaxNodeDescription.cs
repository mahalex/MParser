using System.Xml.Serialization;

namespace SyntaxGenerator
{
    [XmlRoot(ElementName = "Syntax")]
    public class SyntaxDescription
    {
        [XmlElement(ElementName = "Class")]
        public SyntaxNodeDescription[] Nodes { get; set; }
    }
    
    public class SyntaxNodeDescription
    {
        [XmlAttribute("Name")]
        public string ClassName { get; set; }
        [XmlAttribute("BaseClass")]
        public string BaseClassName { get; set; }
        [XmlAttribute("Kind")]
        public string TokenKindName { get; set; }

        [XmlElement(ElementName = "Field")]
        public FieldDescription[] Fields
        {
            get => _fields;
            set => _fields = value ?? new FieldDescription[0];
        }
        
        private FieldDescription[] _fields = new FieldDescription[0];
    }
    
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