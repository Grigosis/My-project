using System;
using System.Collections.Generic;

namespace ClassLibrary1.Inventory
{

    //Dynamic serialization;
    public class DynamicSettings
    {
        public Dictionary<Type, object> StringSettings = new Dictionary<Type, object>();

        public void Add(IDynamicSetting setting)
        {
        }

    }
}