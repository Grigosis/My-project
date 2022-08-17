using Combinator;
using System.Xml.Serialization;
namespace ClassLibrary1.Xml
{
    /// <summary>
    /// Диалог
    /// </summary>
    public class DialogXml
    {
        [XmlAttributeAttribute]
        public string Id;

        public CombinatorNodeXml Combinator;
        [XmlAttributeAttribute]
        public string StartQuestionId;
        //private Quest Quest;
    }
}