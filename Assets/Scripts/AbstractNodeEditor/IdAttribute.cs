using System;

namespace Assets.Scripts.AbstractNodeEditor
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class IdAttribute : Attribute
    {
        
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class IdPointerAttribute : Attribute
    {
        public Type pointerType;

        public IdPointerAttribute(Type pointerType)
        {
            this.pointerType = pointerType;
        }
    }
}