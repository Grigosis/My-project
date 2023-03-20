using System.Xml.Serialization;
using System;

namespace ClassLibrary1.Xml
{
    public class FxAttribute : Attribute {
    
    }
    
    public class AnswerXml
    {
        [XmlAttributeAttribute]
        public string Id; //Уникальный ID ответа
        
        [XmlAttributeAttribute]
        public string AnswerFx; //Id функции возвращающей AnswerArgs
        public string Text; //"Bla lba {MONEY}g? {Nickname}"
        [XmlAttributeAttribute]
        public string Requirements = "STR:20";
        
        [XmlAttributeAttribute]
        public string SelectionFx; // Когда выбрали ответ
        [XmlAttributeAttribute]
        public string NextQuestionId; //Следующий вопрос
    }
    
}