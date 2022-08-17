using System;
using Assets.Scripts.Slime.Sugar;

namespace Assets.Scripts.Slime.Core.Algorythms.Data
{
    public enum FxBool
    {
        Yes,
        No,
        Any
    }

    [Flags]
    public enum RelationShip
    {
        None = 0,
        Ally = 1,
        Enemy = 2,
        Neutral = 4,
        Any = Ally | Enemy | Neutral
    }
    
    public class FxParam
    {
        public readonly string Name;
        public readonly string Value;
        public readonly long? LongValue;
        public readonly double? DoubleValue;
        public readonly double[] DoubleValues;
        public readonly FxBool? Bool;
        public readonly string[] Strings;

        public FxParam(FxParamXml data)
        {
            Name = data.Name;
            Value = data.Value;
            
            if (long.TryParse(Value, out var l))
            {
                LongValue = l;
            }
            if (long.TryParse(Value, out var d))
            {
                DoubleValue = d;
            }
            Strings = Value.Split(",");
            
            var lower = Value.ToLower();
            Bool = ParseFxBool(lower);
            Bool = ParseFxBool(lower);
            DoubleValues = ParseDoubles(Value);
        }

        private static FxBool? ParseFxBool(string lower)
        {
            switch (lower)
            {
                case "yes":
                case "true":
                    return FxBool.Yes;
                case "no":
                case "false":
                    return FxBool.No;
                case "any":
                    return FxBool.Any;
                default:
                    return null;
            }
        }
        
        private static RelationShip ParseRelationShip(string lower)
        {
            var split = lower.Split(" ");
            RelationShip relationShip = RelationShip.None;
            for (int i = 0; i < split.Length; i++)
            {
                switch (lower)
                {
                    case "ally":
                        relationShip |= RelationShip.Ally;
                        break;
                    case "enemy":
                        relationShip |= RelationShip.Enemy;
                        break;
                    case "any":
                    default:
                        relationShip |= RelationShip.Any;
                        break;
                }
            }

            return relationShip;
        }
        
        private static double[] ParseDoubles(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return new double[0];
            }
            
            var split = s.Split(",");
            double[] doubles = new double[split.Length];
            
            for (var i = 0; i < split.Length; i++)
            {
                var doubl = split[i];
                if (double.TryParse(doubl, out var value))
                {
                    doubles[i] = value;
                }
                else
                {
                    return null;
                }
            }

            return doubles;
        }
    }
}