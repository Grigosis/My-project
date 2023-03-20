using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Sugar;

namespace SecondCycleGame.Assets.Scripts.ObjectEditor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class ComboBoxEditorAttribute : Attribute
    {
        public string Path;
        public string[] ExtraVariants;

        public ComboBoxEditorAttribute(string path, string[] extraVariants = null)
        {
            Path = path;
            ExtraVariants = extraVariants;
        }

        public List<string> GetVariants()
        {
            var s = Path.Split(".");
            var cls = s[0];
            var field = s[1];

            var typeByName = R.Instance.GetTypeByName(cls);
            if (typeByName == null)
                return null;

            var infos = typeByName.GetField(field,BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);
            if (infos == null)
                return null;

            var value = infos.GetValue(null);
            if (value == null)
                return null;

            var dict = value as IDictionary;
            
            if (dict == null)
                return null;


            var keys = dict.Keys;
            var list = new List<string>();
            foreach (var key in keys)
            {
                list.Add(key.ToString());
            }

            if (ExtraVariants != null)
            {
                foreach (var extraVariant in ExtraVariants)
                {
                    list.Add(extraVariant);
                }
            }

            return list;
        }
    }
}