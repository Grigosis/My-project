using System;
using System.Collections.Generic;

namespace ClassLibrary1.Inventory
{
    public static class Helper {
        public static DynamicSettings Clone(this DynamicSettings from)
        {
            if (from == null)
            {
                return null;
            }

            var settings = new DynamicSettings();
            settings.StringSettings = new Dictionary<Type, object>(from.StringSettings);
            return settings;
        }    
    }
    
    //Dynamic serialization;
    public class DynamicSettings
    {
        public Dictionary<Type, object> StringSettings = new Dictionary<Type, object>();

        public void Add(IDynamicSetting setting)
        {
        }

    }
}