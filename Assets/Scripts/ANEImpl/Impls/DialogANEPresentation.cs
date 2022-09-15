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
        
        
        public override object OnSerialize()
        {
            return QuestContext; 
        }

        public override void OnLoaded(object obj)
        {
            QuestContext = new QuestContext();
        }

        public override void OnCreatedNew()
        {
            QuestContext = new QuestContext();
        }


        public override void CreateContextMenu()
        {
            AppendToMenu("Add Dialog Starter", (actionEvent, position) => CreateNode(typeof(NPCAnswer), typeof(AnswerANENode), position, null));
            AppendToMenu("Add Dialog", (actionEvent, position) => CreateNode(typeof(QuestDialog), typeof(DialogAneNode), position, null));
            AppendToMenu("Add Combinator", (actionEvent, position) => CreateNode(typeof(CombinatorData), typeof(CombinatorANENode), position, null));
            AppendToMenu("Create Group", (actionEvent, position) => CreateGroup("Unnamed group", new Random().Next(), position));
        }

        public override void RestoreNode(ANENodeState group, ANEGroup groupNode)
        {
            var data = group.Data;
            if (data is QuestDialog qd)
            {
                CreateNode(qd, typeof(DialogAneNode), group.Position, groupNode);
            }
            if (data is CombinatorData cs)
            {
                CreateNode(cs, typeof(CombinatorANENode), group.Position, groupNode);
            }
            if (data is NPCAnswer ass)
            {
                CreateNode(ass, typeof(AnswerANENode), group.Position, groupNode);
            }
        }
        
        public override void OnNewObjectCreated(object o)
        {
            if (o is QuestDialog qd)
            {
                var a = new QuestAnswer(); 
                a.Text = "Hi!";
                a.AnswerFx = "Test";
                qd.Answers.Add(a);
            }
            
            if (o is CombinatorData cs)
            {
                cs.Value = "1";
                cs.Fx = "Constant";
            }
            
            if (o is NPCAnswer ass)
            {
                ass.Text = "Starter";
            }
        }
    }
}