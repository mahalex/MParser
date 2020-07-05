using System.Xml.Serialization;

namespace SyntaxGenerator
{
    [XmlRoot(ElementName = "Syntax")]
    public class SyntaxDescription
    {
        [XmlElement(ElementName = "Class")]
        public SyntaxNodeDescription[] Nodes { get; set; }
    }
}