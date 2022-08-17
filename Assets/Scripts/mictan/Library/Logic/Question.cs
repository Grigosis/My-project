using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary1.Xml;

namespace ClassLibrary1.Logic
{
    public class Question
    {
        public QuestionXml Xml;

        public QuestionArgsFx QuestionFx;

        public Answer[] answers;
    }
}