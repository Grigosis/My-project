using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Assets.Scripts.Slime.Sugar;
using UnityEngine;

namespace BlockEditor.Serialization
{
    public class ClassCache
    {
            public static Dictionary<string, ClassCache> Data = new Dictionary<string, ClassCache>();

            public static ClassCache Get(Type t)
            {
                var n = t.FullName;
                if (n == null)
                {
                    return new ClassCache(t);
                } 
                
                lock (Data)
                {
                    if (!Data.ContainsKey(n))
                    {
                        Data[n] = new ClassCache(t);
                    }
                    return Data[n];
                }
            }
            
            public Dictionary<string, Tuple<MemberInfo, XmlAttributeAttribute>> AttributeFields = new Dictionary<string, Tuple<MemberInfo, XmlAttributeAttribute>>();
            public Dictionary<string, MemberInfo> Fields = new Dictionary<string,MemberInfo>();
            public Dictionary<string, MemberInfo> FieldsAndAttrs = new Dictionary<string,MemberInfo>();
            public Dictionary<string, MethodInfo> ShouldSerializeDictionary = new Dictionary<string,MethodInfo>();

            private ClassCache(Type t)
            {
                try
                {
                    Populate(t);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error with type {t} {e}");
                }
            }

            private void Populate(Type t)
            {
                var all = new List<MemberInfo>();
                all.AddRange(t.GetFields(BindingFlags.Public | BindingFlags.Instance));
                all.AddRange(t.GetProperties(BindingFlags.Public | BindingFlags.Instance));
                foreach (var f in all)
                {
                    try
                    {
                        if (f is PropertyInfo property)
                        {
                            if (property.SetMethod == null || property.GetMethod == null) continue;
                        }
                        if (f.GetAttribute<XmlIgnoreAttribute>() != null) continue;
                        var attr = f.GetCustomAttribute<XmlAttributeAttribute>();
                        var attr2 = f.GetCustomAttribute<XmlElementAttribute>();
                        if (attr != null)
                        {
                            var key = attr.AttributeName == "" ? f.Name : attr.AttributeName;
                            AttributeFields.Add(key, new Tuple<MemberInfo, XmlAttributeAttribute>(f, attr));
                        }
                        else
                        {
                            var key = f.Name;
                            if (attr2 != null) key = attr2.ElementName == "" ? f.Name : attr2.ElementName;
                            Fields.Add(key, f);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Error while processing {f.Name} {f.DeclaringType} {e}");
                    }
                }

                foreach (var f in t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic))
                {
                    if (!f.Name.StartsWith("ShouldSerialize")) continue;
                    if (f.ReturnType != typeof(bool)) continue;

                    var name = f.Name.Substring("ShouldSerialize".Length);
                    ShouldSerializeDictionary[name] = f;
                }

                foreach (var v in Fields)
                {
                    FieldsAndAttrs[v.Key] = v.Value;
                }
                foreach (var v in AttributeFields)
                {
                    FieldsAndAttrs[v.Key] = v.Value.Item1;
                }
            }

            public MemberInfo GetAttributeMember(string attrName)
            {
                if (AttributeFields.TryGetValue(attrName, out var v))
                {
                    return v.Item1;
                }
                return null;
            }
            public MemberInfo GetMember(string attrName)
            {
                if (Fields.TryGetValue(attrName, out var v))
                {
                    return v;
                }
                return null;
            }
            
            

            public bool ShouldSerialize(object o, MemberInfo member)
            {
                if (!ShouldSerializeDictionary.ContainsKey(member.Name))
                {
                    return true;
                }

                if (o == null)
                {
                    var wtf = 0;
                }
                var result = (bool)ShouldSerializeDictionary[member.Name].Invoke(o, new object[0]);
                return result;
            }
        }
}