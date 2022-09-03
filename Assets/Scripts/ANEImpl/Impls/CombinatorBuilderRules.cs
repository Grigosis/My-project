using System;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.AbstractNodeEditor.Impls;
using Combinator;
using DS.Windows;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;

namespace SecondCycleGame.Assets.Scripts.ANEImpl.Impls
{
    public class CombinatorBuilderRules : Combinator.CombinatorBuilder.ICombinatorBuilderRules
    {
        protected ANEGraph Graph;
        protected QuestContext Context;

        public CombinatorBuilderRules(QuestContext context, ANEGraph graph = null)
        {
            Graph = graph;
            Context = context;
        }


        public object ParseConstant(CombinatorScriptable data, Type shouldBeTypeOf)
        {
            if (shouldBeTypeOf == typeof(double)) {
                return double.Parse(data.Value);
            }

            if (shouldBeTypeOf == typeof(string)) {
                return data.Value;
            }

            throw new Exception($"Missing parser for type {shouldBeTypeOf}");
        }

        public ISubscription Subscriber(CombinatorScriptable data, ICombinator combinator)
        {
            var dataValue = data.Value;
            if (!Context.Subscribables.TryGetValue(dataValue, out var subscribable))
            {
                throw new Exception($"Not found context [{dataValue}]");
            }

            return new SubscribableSub(subscribable, combinator);
        }

        public void OnCombinatorCreate(CombinatorScriptable data, ICombinator combinator)
        {
            if (Graph != null)
            {
                CombinatorANENode.AttachCombinator(Graph, data, combinator);
            }
        }
    }
}