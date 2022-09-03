using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.AbstractNodeEditor.Impls;
using Assets.Scripts.Slime.Sugar;
using ROR.Core.Serialization;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace SecondCycleGame.Assets.Scripts.ANEImpl.Impls
{
    public class DialogANEPresentation : ANEIPresentation
    {
        public QuestContext QuestContext;
        
        
        public override Object OnSerialize()
        {
            return QuestContext; 
        }

        public override void OnLoaded(object obj)
        {
            //QuestContext = (QuestContext)obj;
        }

        public override void OnCreatedNew()
        {
            QuestContext = ScriptableObject.CreateInstance<QuestContext>();
        }


        public override void CreateContextMenu()
        {
            AppendToMenu("Add Dialog", (actionEvent, position) => CreateNode(typeof(QuestDialog), typeof(DialogAneNode), position, null));
            AppendToMenu("Add Combinator", (actionEvent, position) => CreateNode(typeof(CombinatorScriptable), typeof(CombinatorANENode), position, null));
            AppendToMenu("Create Group", (actionEvent, position) => CreateGroup("Unnamed group", new Random().Next(), position));
        }

        public override void RestoreNode(ANENodeState group, ANEGroup groupNode)
        {
            var data = group.Data;
            if (data is QuestDialog qd)
            {
                CreateNode(qd, typeof(DialogAneNode), group.Position, groupNode);
            }
            if (data is CombinatorScriptable cs)
            {
                CreateNode(cs, typeof(CombinatorANENode), group.Position, groupNode);
            }
        }
        
        public override void OnNewObjectCreated(Object o)
        {
            if (o is QuestDialog qd)
            {
                var a = ScriptableObject.CreateInstance<QuestAnswer>(); 
                a.Text = "Hi!";
                a.AnswerFx = "Test";
                qd.Answers.Add(a);
            }
            
            if (o is CombinatorScriptable cs)
            {
                cs.Value = new Random().NextString(8);
                cs.Fx = new Random().NextString(8);
            }
        }
    }
}