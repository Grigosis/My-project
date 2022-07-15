using System;
using System.Xml.Serialization;
using RPGFight.Core;
using UnityEngine;

namespace RPGFight
{
    public class Attrs
    {
        [XmlAttribute] public double HP_NOW = 0;
        [XmlAttribute] public double HP_MAX = 0;
        [XmlAttribute] public double HP_REGEN = 0;
        [XmlAttribute] public double EP_NOW = 0;
        [XmlAttribute] public double EP_MAX = 0;
        [XmlAttribute] public double EP_REGEN = 0;
        [XmlAttribute] public double HIT_CLOSE_RANGE = 0;
        [XmlAttribute] public double HIT_FAR_RANGE = 0;
        [XmlAttribute] public double DODGE_CLOSE_RANGE = 0;
        [XmlAttribute] public double DODGE_FAR_RANGE = 0;
        [XmlAttribute] public double CRIT_CHANCE = 0;
        [XmlAttribute] public double CRIT_MLT = 0;
        [XmlAttribute] public double MOVESPEED = 0;
        [XmlAttribute] public double INITIATIVE = 0;

        public Stats STATS;
        public Perks PERKS;

        public Element CRUSH;
        public Element PIERCE;
        public Element CUT;
        public Element FIRE;
        public Element COLD;
        public Element POISON;
        public Element GRAVI;

        public static Attrs NewWithSubs()
        {
            var attrs = new Attrs();
            attrs.STATS = new Stats();
            attrs.PERKS = new Perks();
            attrs.CRUSH = new Element();
            attrs.PIERCE = new Element();
            attrs.CUT = new Element();
            attrs.FIRE = new Element();
            attrs.COLD = new Element();
            attrs.POISON = new Element();
            attrs.GRAVI = new Element();

            return attrs;
        }

        public void CalculateFinalValues()
        {
            using (new ChangeAttrs(this))
            {
                Balance.CalculateFinalValues(this);
            }
        }



        public void Sum(Attrs other)
        {
            HP_NOW  += other.HP_NOW;
            HP_MAX  += other.HP_MAX;
            HP_REGEN  += other.HP_REGEN;
            EP_NOW  += other.EP_NOW;
            EP_MAX  += other.EP_MAX;
            EP_REGEN  += other.EP_REGEN;
            HIT_CLOSE_RANGE  += other.HIT_CLOSE_RANGE;
            HIT_FAR_RANGE  += other.HIT_FAR_RANGE;
            DODGE_CLOSE_RANGE  += other.DODGE_CLOSE_RANGE;
            DODGE_FAR_RANGE  += other.DODGE_FAR_RANGE;
            CRIT_CHANCE  += other.CRIT_CHANCE;
            CRIT_MLT  += other.CRIT_MLT;
            MOVESPEED  += other.MOVESPEED;
            INITIATIVE  += other.INITIATIVE;

            STATS.Sum(other.STATS);
            PERKS.Sum(other.PERKS);

            CRUSH.Sum(other.CRUSH);
            PIERCE.Sum(other.PIERCE);
            CUT.Sum(other.CUT);
            FIRE.Sum(other.FIRE);
            COLD.Sum(other.COLD);
            POISON.Sum(other.POISON);
            GRAVI.Sum(other.GRAVI);
        }

        public Element GetElement(int attackId)
        {
            switch (attackId)
            {
                case 0: return CRUSH;
                case 1: return PIERCE;
                case 2: return CUT;
                case 3: return FIRE;
                case 4: return COLD;
                case 5: return POISON;
                case 6: return GRAVI;
                default: throw new Exception($"Element with name [{attackId}] not found");
            }
        }

        public static int GetElementId(string attackName)
        {
            switch (attackName)
            {
                case "CRUSH": return 0;
                case "PIERCE": return 1;
                case "CUT": return 2;
                case "FIRE": return 3;
                case "COLD": return 4;
                case "POISON": return 5;
                case "GRAVI": return 6;
                default: throw new Exception($"Element with name [{attackName}] not found");
            }
        }

        public static Color GetElementColor(int attackId)
        {
            switch (attackId)
            {
                case 0: return Color.red;
                case 1: return Color.red;
                case 2: return Color.red;
                case 3: return Color.red;
                case 4: return Color.red;
                case 5: return Color.red;
                case 6: return Color.red;
                default: throw new Exception($"Element with name [{attackId}] not found");
            }
        }

        public static string GetElementName(int attackId)
        {
            switch (attackId)
            {
                case 0: return "CRUSH";
                case 1: return "PIERCE";
                case 2: return "CUT";
                case 3: return "FIRE";
                case 4: return "COLD";
                case 5: return "POISON";
                case 6: return "GRAVI";
                default: throw new Exception($"Element with name [{attackId}] not found");
            }
        }

        public override string ToString()
        {
            return
                $"HP: {HP_NOW}/{HP_MAX} regen{HP_REGEN} \r\n" +
                $"EP: {EP_NOW}/{EP_MAX} regen{EP_REGEN} \r\n" +
                $"HIT: {HIT_CLOSE_RANGE}/{HIT_FAR_RANGE} \r\n" +
                $"DODGE: {DODGE_CLOSE_RANGE}/{DODGE_FAR_RANGE} \r\n" +
                $"CRIT: {CRIT_CHANCE}% x{CRIT_MLT} \r\n" +
                $"MOVE: {MOVESPEED} \r\n" +
                $"INI: {INITIATIVE} \r\n" +
                $"STATS: {STATS} \r\n" +
                $"PERKS: {PERKS} \r\n" +
                $"CRUSH: {CRUSH} \r\n" +
                $"PIERCE: {PIERCE} \r\n" +
                $"CUT: {CUT} \r\n" +
                $"FIRE: {FIRE} \r\n" +
                $"COLD: {COLD} \r\n" +
                $"POISON: {POISON} \r\n" +
                $"GRAVI: {GRAVI} ";
        }
    }

    public class Element
    {
        [XmlAttribute] public double ATK_MIN_ABS = 0;
        [XmlAttribute] public double ATK_MAX_ABS = 0;
        [XmlAttribute] public double ATK_MIN_MLT = 0;
        [XmlAttribute] public double ATK_MAX_MLT = 0;
        [XmlAttribute] public double DEF_ABS = 0;
        [XmlAttribute] public double DEF_MLT = 0;
        [XmlAttribute] public double SPECIAL_1 = 0;
        [XmlAttribute] public double SPECIAL_2 = 0;

        public void Sum(Element other)
        {
            ATK_MIN_ABS += other.ATK_MIN_ABS;
            ATK_MAX_ABS += other.ATK_MAX_ABS;
            ATK_MIN_MLT += other.ATK_MIN_MLT;
            ATK_MAX_MLT += other.ATK_MAX_MLT;
            DEF_ABS += other.DEF_ABS;
            DEF_MLT += other.DEF_MLT;
            SPECIAL_1 += other.SPECIAL_1;
            SPECIAL_2 += other.SPECIAL_2;
        }

        public override string ToString()
        {
            return
                $"ATK: [{ATK_MIN_ABS}x{ATK_MIN_MLT} - {ATK_MAX_ABS}x{ATK_MAX_MLT}] " +
                $"DEF: [{DEF_ABS} | {DEF_MLT}%] " +
                $"SPECIAL: {SPECIAL_1} | {SPECIAL_2}";
        }
    }

    public class Stats
    {
        [XmlAttribute] public double STR = 0;
        [XmlAttribute] public double AGI = 0;
        [XmlAttribute] public double END = 0;
        [XmlAttribute] public double INT = 0;
        [XmlAttribute] public double WIL = 0;
        [XmlAttribute] public double PER = 0;

        public void Sum(Stats other)
        {
            STR += other.STR;
            AGI += other.AGI;
            END += other.END;
            INT += other.INT;
            WIL += other.WIL;
            PER += other.PER;
        }

        public bool IsLess(Stats other)
        {
            return  STR < other.STR &&
                    AGI < other.AGI &&
                    END < other.END &&
                    INT < other.INT &&
                    WIL < other.WIL &&
                    PER < other.PER;
        }

        public override string ToString()
        {
            return $"STR: {STR } " +
                   $"AGI: {AGI } " +
                   $"END: {END } " +
                   $"INT: {INT } " +
                   $"WIL: {WIL } " +
                   $"PER: {PER }";
        }
    }

    public class Perks
    {
        [XmlAttribute] public double CLOSE_WEAPON;
        [XmlAttribute] public double RANGED_WEAPON;
        [XmlAttribute] public double SCI_WEAPON;
        [XmlAttribute] public double HACKING;
        [XmlAttribute] public double TECH;
        [XmlAttribute] public double MEDICINE;

        public void Sum(Perks other)
        {
            CLOSE_WEAPON += other.CLOSE_WEAPON;
            RANGED_WEAPON += other.RANGED_WEAPON;
            SCI_WEAPON += other.SCI_WEAPON;
            HACKING += other.HACKING;
            TECH += other.TECH;
            MEDICINE += other.MEDICINE;
        }

        public override string ToString()
        {
            return $"CLOSE_WEAPON: {CLOSE_WEAPON} " +
                   $"RANGED_WEAPON: {RANGED_WEAPON} " +
                   $"SCI_WEAPON: {SCI_WEAPON} " +
                   $"HACKING: {HACKING} " +
                   $"TECH: {TECH} " +
                   $"MEDICINE: {MEDICINE }";
        }
    }



    public class ChangeAttrs : IDisposable
    {
        public Attrs Attrs;
        private double HP_Ratio;

        public ChangeAttrs(Attrs attrs)
        {
            Attrs = attrs;
            HP_Ratio = attrs.HP_NOW / Attrs.HP_MAX;
        }

        public void Dispose()
        {
            Attrs.HP_NOW = HP_Ratio * Attrs.HP_MAX;
        }
    }
}