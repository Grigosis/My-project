﻿using System;
using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.AbstractNodeEditor.Impls;
using Combinator;
using DS.Windows;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using UnityEngine;
using UnityEngine.UIElements;

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


        public object ParseConstant(CombinatorData data, Type shouldBeTypeOf)
        {
            if (shouldBeTypeOf == typeof(double)) {
                return double.Parse(data.Value);
            }

            if (shouldBeTypeOf == typeof(string)) {
                return data.Value;
            }

            if (shouldBeTypeOf == typeof(bool)) {
                var v = data.Value.ToLower();
                return new List<string>() { "true", "1", "t" }.Contains(v);
            }

            throw new Exception($"Missing parser for type {shouldBeTypeOf}");
        }

        public ISubscription Subscriber(CombinatorData data, ICombinator combinator)
        {
            var dataValue = data.Value;
            if (String.IsNullOrEmpty(dataValue))
            {
                return null;
            }
            
            if (!Context.Subscribables.TryGetValue(dataValue, out var subscribable))
            {
                OnError(data);
                throw new Exception($"Not found context [{dataValue}]");
            }

            return new SubscribableSub(subscribable, combinator);
        }

        public void OnCombinatorCreate(CombinatorData data, ICombinator combinator)
        {
            if (Graph != null)
            {
                DialogANEPresentation.AttachCombinator(Graph, data, combinator);
            }
        }

        public void OnError(CombinatorData xml)
        {
            var node = Graph.NodesAndData.Get(xml);
            if (node == null)
            {
                Debug.LogError("Node not found:" + node);
                return;
            }

            if (node is CombinatorANENode nodee)
            {
                nodee.style.backgroundColor = new StyleColor(Color.red);
            }
        }
    }
}