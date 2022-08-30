﻿using System.Collections.Generic;
using System.Text;
using Assets.Scripts.AbstractNodeEditor;

namespace Assets.Scripts.Library {
    public class CombinatorFunctions {
        public static string Concat(CombinatorScriptable xml, List<string> parts) {
            var sb = new StringBuilder();
            foreach (var part in parts) {
                sb.Append(part);
            }
            return sb.ToString();
        }

        public static double Mlt(CombinatorScriptable xml, List<double> parts) {
            var mlt = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                mlt *= parts[i];
            }

            return mlt;
        }

        public static double Sum(CombinatorScriptable xml, List<double> parts) {
            var sum = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                sum += parts[i];
            }

            return sum;
        }

        public static bool IsMore(CombinatorScriptable xml, List<double> parts) {
            var last = parts[0];
            for(int i = 1; i < parts.Count; i++) {
                if(last <= parts[i]) {//! IsMore
                    return false;
                }
                last = parts[i];
            }

            return true;
        }

        public static bool Eq(CombinatorScriptable xml, List<double> parts) {
            var first = parts[0];
            for (var i = 1; i < parts.Count; i++) {
                if (parts[i] != first) {
                    return false;
                }
            }

            return true;
        }

        public static bool And(CombinatorScriptable xml, List<bool> parts) {
            for (var i = 0; i < parts.Count; i++) {
                if (!parts[i]) {
                    return false;
                }
            }

            return true;
        }

        public static bool Or(CombinatorScriptable xml, List<bool> parts) {
            for (var i = 0; i < parts.Count; i++) {
                if (parts[i]) {
                    return true;
                }
            }

            return false;
        }

        public static string ToStr(CombinatorScriptable xml, List<double> parts) {
            return parts[0].ToString();
        }
    }
}
