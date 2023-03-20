using System.Xml.Serialization;
using System.Collections.Generic;

namespace ClassLibrary1.Xml
{
    
    /// <summary>
    /// Вопрос в диалоге
    /// </summary>
    public class QuestionXml
    {
        [XmlAttributeAttribute]
        public string Id;
        public string Text;//private?
        [XmlAttributeAttribute]
        public string TextArgsFx; //Смотри Library
        public List<string> Answers; // Id возможных ответов
    }
}