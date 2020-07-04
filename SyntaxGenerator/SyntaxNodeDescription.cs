using System.Xml.Serialization;

namespace SyntaxGenerator
{
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
}