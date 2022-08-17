using System;

namespace Combinator
{
    public class CombinatorBuilder
    {
        public static ICombinator Build(CombinatorNodeXml xml, Type parentType, Func<CombinatorNodeXml, Type, object> constantParser, Action<CombinatorNodeXml, ICombinator> subscriber)
        {
            return BuildInternal(xml, parentType, constantParser, subscriber);
        }
        
        private static ICombinator BuildInternal(CombinatorNodeXml xml, Type parentType, Func<CombinatorNodeXml, Type, object> constantParser, Action<CombinatorNodeXml, ICombinator> subscriber)
        {
            if (xml.Fx == "Constant")
            {
                var t = typeof(CombinatorConstNode<,>).MakeGenericType(typeof(CombinatorNodeXml), parentType);
                var combi = (ICombinator)Activator.CreateInstance(t);
                combi.SetObject(xml);
                IConstCombinator combinator = (IConstCombinator)combi;

                var value = constantParser(xml, parentType);
                combinator.SetValue(value);
                combi.NodeDebugName = $"{value}";
                return combi;
            }
            
            var info = F.Functions[xml.Fx];
            var returnType = info.Item1;
            var childTypes = info.Item2;
            var context = info.Item3;
            var fx = info.Item4;

            if (context == null)
            {
                if (childTypes == null)
                {
                    if (xml.Nodes != null && xml.Nodes.Length > 0)
                    {
                        throw new Exception("SubNodes are not allowed for this node");
                    }
                
                    var t = typeof(CombinatorSingleNode<,>).MakeGenericType(typeof(CombinatorNodeXml), returnType);
                    var combi = (ICombinator)Activator.CreateInstance(t);
                    subscriber(xml, combi);
                    combi.SetFx(fx);
                    combi.SetObject(xml);
                    combi.NodeDebugName = $"{xml.Fx}(`{xml.Value}`)";
                    return combi;
                }
                else
                {
                    var t = typeof(CombinatorMultiNode<,,>).MakeGenericType(typeof(CombinatorNodeXml), childTypes, returnType);
                    var combi = (IMultiCombinator)Activator.CreateInstance(t);
                    subscriber(xml, combi);
                    combi.SetFx(fx);
                    combi.SetObject(xml);
                    combi.NodeDebugName = $"{xml.Fx}";
                    
                    bool IsContexted = false;
                    foreach (var node in xml.Nodes)
                    {
                        var child = Build(node, childTypes, constantParser, subscriber);
                        combi.AddNode(child);
                        IsContexted |= child.IsDependent;
                    }
                    combi.IsDependent = IsContexted;
                    return combi;
                }
            }
            else
            {
                if (childTypes == null)
                {
                    if (xml.Nodes != null && xml.Nodes.Length > 0)
                    {
                        throw new Exception("SubNodes are not allowed for this node");
                    }
                
                    var t = typeof(ContextCombinatorSingleNode<,,>).MakeGenericType(typeof(CombinatorNodeXml), context, returnType);
                    var combi = (ICombinator)Activator.CreateInstance(t);
                    subscriber(xml, combi);
                    combi.SetFx(fx);
                    combi.SetObject(xml);
                    combi.NodeDebugName = $"{xml.Fx}(`{xml.Value}`)";
                    combi.IsDependent = true;
                    return combi;
                }
                else
                {
                    var t = typeof(ContextCombinatorMultiNode<,,,>).MakeGenericType(typeof(CombinatorNodeXml), context, childTypes, returnType);
                    var combi = (IMultiCombinator)Activator.CreateInstance(t);
                    subscriber(xml, combi);
                    combi.SetFx(fx);
                    combi.SetObject(xml);
                    combi.IsDependent = true;
                    combi.NodeDebugName = $"{xml.Fx}";
                    foreach (var node in xml.Nodes)
                    {
                        var child = Build(node, childTypes, constantParser, subscriber);
                        combi.AddNode(child);
                    }
                
                    return combi;
                }
            }
            
        }
    }
}