using System;

namespace Assets.Scripts.Slime.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class FxAttribute : Attribute
    {
        private string AttributeName;

        public FxAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }
    }
}