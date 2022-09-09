using System;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;

namespace Combinator
{
    public class CombinatorBuilder
    {
        public interface ICombinatorBuilderRules
        {
            public object ParseConstant(CombinatorData data, Type shouldBeTypeOf);
            public ISubscription Subscriber(CombinatorData data, ICombinator combinator);
            public void OnCombinatorCreate(CombinatorData data, ICombinator combinator);
        }

        

        public static ICombinator Build(CombinatorData xml, Type parentType, ICombinatorBuilderRules builderRules)
        {
            return BuildInternal(xml, parentType, builderRules);
        }
        
        private static ICombinator BuildInternal(CombinatorData xml, Type parentType, ICombinatorBuilderRules builderRules)
        {
            if (xml.Fx == "Constant")
            {
                var t = typeof(CombinatorConstNode<,>).MakeGenericType(typeof(CombinatorData), parentType);
                var combi = (ICombinator)Activator.CreateInstance(t);
                combi.SetObject(xml);
                IConstCombinator combinator = (IConstCombinator)combi;

                var value = builderRules.ParseConstant(xml, parentType);
                combinator.SetValue(value);
                combi.NodeDebugName = $"{value}";
                builderRules.OnCombinatorCreate(xml, combi);
                return combi;
            }
            
            var info = F.Functions[xml.Fx];
            var returnType = info.OutType;
            var childTypes = info.InType;
            var context = info.ContextType;
            var fx = info.Func;

            if (context == null)
            {
                if (childTypes == null)
                {
                    if (xml.Nodes != null && xml.Nodes.Count > 0)
                    {
                        throw new Exception("SubNodes are not allowed for this node");
                    }
                
                    var t = typeof(CombinatorSingleNode<,>).MakeGenericType(typeof(CombinatorData), returnType);
                    var combi = (ICombinator)Activator.CreateInstance(t);
                    combi.SetSubscription(builderRules.Subscriber(xml, combi));
                    combi.SetFx(fx);
                    combi.SetObject(xml);
                    combi.NodeDebugName = $"{xml.Fx}(`{xml.Value}`)";
                    builderRules.OnCombinatorCreate(xml, combi);
                    return combi;
                }
                else
                {
                    var t = typeof(CombinatorMultiNode<,,>).MakeGenericType(typeof(CombinatorData), childTypes, returnType);
                    var combi = (IMultiCombinator)Activator.CreateInstance(t);
                    combi.SetSubscription(builderRules.Subscriber(xml, combi));
                    combi.SetFx(fx);
                    combi.SetObject(xml);
                    combi.NodeDebugName = $"{xml.Fx}";
                    
                    bool IsContexted = false;
                    foreach (var node in xml.Nodes)
                    {
                        if (node == null) continue;
                        var child = Build(node, childTypes, builderRules);
                        combi.AddNode(child);
                        IsContexted |= child.IsDependent;
                    }
                    combi.IsDependent = IsContexted;
                    builderRules.OnCombinatorCreate(xml, combi);
                    return combi;
                }
            }
            else
            {
                if (childTypes == null)
                {
                    if (xml.Nodes != null && xml.Nodes.Count > 0)
                    {
                        throw new Exception("SubNodes are not allowed for this node");
                    }
                
                    var t = typeof(ContextCombinatorSingleNode<,,>).MakeGenericType(typeof(CombinatorData), context, returnType);
                    var combi = (ICombinator)Activator.CreateInstance(t);
                    combi.SetSubscription(builderRules.Subscriber(xml, combi));
                    combi.SetFx(fx);
                    combi.SetObject(xml);
                    combi.NodeDebugName = $"{xml.Fx}(`{xml.Value}`)";
                    combi.IsDependent = true;
                    builderRules.OnCombinatorCreate(xml, combi);
                    return combi;
                }
                else
                {
                    var t = typeof(ContextCombinatorMultiNode<,,,>).MakeGenericType(typeof(CombinatorData), context, childTypes, returnType);
                    var combi = (IMultiCombinator)Activator.CreateInstance(t);
                    combi.SetSubscription(builderRules.Subscriber(xml, combi));
                    combi.SetFx(fx);
                    combi.SetObject(xml);
                    combi.IsDependent = true;
                    combi.NodeDebugName = $"{xml.Fx}";
                    foreach (var node in xml.Nodes)
                    {
                        if (node == null) continue;
                        var child = Build(node, childTypes, builderRules);
                        combi.AddNode(child);
                    }
                    builderRules.OnCombinatorCreate(xml, combi);
                    return combi;
                }
            }
            
        }
    }
}