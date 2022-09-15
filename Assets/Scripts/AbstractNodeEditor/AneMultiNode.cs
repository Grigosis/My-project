using System.Collections.Generic;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.AbstractNodeEditor.Impls;
using RPGFight.Library;
using SecondCycleGame.Assets.Scripts.ANEImpl.Impls;
using SecondCycleGame.Assets.Scripts.ANEImpl.Views;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace SecondCycleGame.Assets.Scripts.AbstractNodeEditor
{
    public abstract class ANEMultiNode<DATA, DATA2, VIEW> : DefaultANENode<DATA>, IRowListener<DATA2> where DATA : new() where DATA2 : new() where VIEW : RowView<DATA, DATA2>, new()
    {
        protected DATA Data => (DATA)NodeData;
        protected DoubleDictionary<DATA2, VIEW> Data2ToPorts = new DoubleDictionary<DATA2, VIEW>();
        
        //UI
        protected Button addSubnodeButton;
        protected VisualElement subnodesContainer;


        public ANEMultiNode(string path) : base(path) { }

        
        
        protected abstract List<DATA2> GetSubNodes();
        
        
        public virtual void OnSubNodeDelete(VisualElement view, DATA2 onDelete)
        {
            var rw = (view as RowView<DATA, DATA2>);
            if (rw == null) return;
            if (rw.EPort.connected)
            {
                Graph.DeleteElements(rw.EPort.connections);
            }
            Graph.RemoveElement(rw.EPort);
            
            GetSubNodes().Remove(onDelete);
            Data2ToPorts.Remove(onDelete);
            view.parent.Remove(view);
        }

        public virtual void OnSubNodeValueChanged(VisualElement view, DATA2 oldValue, DATA2 newValue)
        {
            var indexOf = view.parent.hierarchy.IndexOf(view);
            GetSubNodes()[indexOf] = newValue;
            
            if (oldValue != null)
            {
                Data2ToPorts.Remove(oldValue);
            }
            if (newValue != null)
            {
                Data2ToPorts.Add(newValue, (VIEW)view);
            }
        }

        public virtual void OnEditRequest(VisualElement view, DATA2 onDelete)
        {
            Graph.GetEditor().RequestEditObject(onDelete, OnEditorFinished);
        }
        
        public virtual void OnMoveRequest(VisualElement view, DATA2 onDelete, int direction)
        {
            var list = GetSubNodes();
            var i = list.IndexOf(onDelete);
            if (i == -1) return;
            if (direction == 0) return;

            int i2;

            if (direction > 0)
            {
                if (i+1 >= list.Count) return;
                i2 = i + 1;
            } else 
            {
                if (i-1 < 0) return;
                i2 = i - 1;
            }

            var data2 = list[i2];

            list[i2] = onDelete;
            list[i] = data2;

            var parent = view.parent.hierarchy;
            parent.RemoveAt(i);
            parent.Insert(i2, view);

        }

        public override void CreateGUI()
        {
            base.CreateGUI();
            addSubnodeButton = this.Q<Button>("add-subnode");
            subnodesContainer = this.Q<VisualElement>("subnodes-container");
            addSubnodeButton.clickable.clicked += OnAddSubNodeClicked;

            foreach (var answer in GetSubNodes())
            {
                CreateAnswerView(answer, false);
            }
        }
        
        protected virtual void OnAddSubNodeClicked()
        {
            var answer = new DATA2();
            Graph.Presentation.OnNewObjectCreated(answer);
            GetSubNodes().Add(answer);
            CreateAnswerView(answer, true);
        }

        protected virtual void CreateAnswerView(DATA2 answer, bool isNew)
        {
            VIEW answerView = new VIEW();
            answerView.Init(Data, answer, this);
            Data2ToPorts.Add(answer, answerView);
            subnodesContainer.Add(answerView);
        }

        protected virtual void OnEditorFinished(object obj)
        {
            Debug.LogError("OnEditorFinished:" + obj);
            var view = Data2ToPorts.Get((DATA2)obj);
            if (view != null)
            {
                view.UpdateUI();
            }

            UpdateUI();
        }
    }
}