using Combinator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Library {
    class CombinatorFunctions {
        static string Concat(CombinatorNodeXml xml, List<string> parts) {
            var sb = new StringBuilder();
            foreach (var part in parts) {
                sb.Append(part);
            }
            return sb.ToString();
        }

        static double Mlt(CombinatorNodeXml xml, List<double> parts) {
            var mlt = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                mlt *= parts[i];
            }

            return mlt;
        }

        static double Sum(CombinatorNodeXml xml, List<double> parts) {
            var sum = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                sum += parts[i];
            }

            return sum;
        }

        static string ToStr(CombinatorNodeXml xml, List<double> parts) {
            return parts[0].ToString();
        }

        public static void Register() {
            F.RegisterMulti<CombinatorNodeXml, string, string>("Concat", Concat);
            F.RegisterMulti<CombinatorNodeXml, double, double>("Mlt", Mlt);
            F.RegisterMulti<CombinatorNodeXml, double, double>("Sum", Sum);
            F.RegisterMulti<CombinatorNodeXml, double, string>("ToStr", ToStr);
        }

        public static object Parser(CombinatorNodeXml xml, Type type) {
            if (type == typeof(double)) {
                return double.Parse(xml.Value);
            }

            if (type == typeof(string)) {
                return xml.Value;
            }

            throw new Exception($"Missing parser for type {type}");
        }

        public static void Subscriber(CombinatorNodeXml xml, ICombinator combinator) {//TODO замыкание? fkdjODSKJV
            //combinator.OnChanged += CombinatorOnOnChanged;
            //if (xml.Fx == "CHAR_VALUE") {
            //    CharValues[xml.Value].OnChanged += (a, b) => combinator.MarkForRecalculate();
            //}
            //if (xml.Fx == "NPC_VALUE")
            //{
            //    NPCValues[xml.Value].OnChanged += (a, b) => combinator.MarkForRecalculate();
            //}
        }
    }
}
