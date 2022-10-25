using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Assets.Scripts.AbstractNodeEditor;
using Assets.Scripts.Slime.Core;
using Assets.Scripts.Slime.Sugar;
using ClassLibrary1.Logic;
using ROR.Core.Serialization.Json;
using SecondCycleGame.Assets.Scripts.ObjectEditor;
using Slime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace ROR.Core.Serialization
{

    [Serializable]
    public class QuestDialog : Linkable
    {
        [HideInInspector]
        [field:NonSerialized]
        public CombinatorData VisibilityCombinator;

        [SerializeField]
        public string Id;
        
        [Multiline]
        [SerializeField]
        public string Text;
        
        [Multiline]
        [SerializeField]
        public string Sounds;
        
        [SerializeField]
        [ComboBoxEditor("F.QuestionArgsFx")]
        public string TextArgsFx;
        
        [HideInInspector]
        [field:NonSerialized]
        public List<QuestAnswer> Answers = new List<QuestAnswer>();

        [HideInInspector]
        [SerializeField]
        public string VisibilityCombinatorGuid;

        [HideInInspector]
        [SerializeField]
        public List<string> AnswersGUIDS = new List<string>();
        
        [HideInInspector] 
        public bool CanReturnBack;
        
        
        public HashSet<QuestDialog> GetConnectedDialogsDialogTree(QuestDialog dialog, HashSet<QuestDialog> set)
        {
            foreach (var questAnswer in dialog.Answers)
            {
                if (questAnswer.NextQuestionDialog == null) continue;
                if (!set.Add(questAnswer.NextQuestionDialog)) continue;

                GetConnectedDialogsDialogTree(questAnswer.NextQuestionDialog, set);
            }

            return set;
        }
        
        
        public void AttachSounds()
        {
            var lines = Sounds.Split("\r\n", "\n");
            //new Thread(new ThreadStart(() =>
            //{
                foreach (var line in lines)
                {
                    Sugar.PlaySound(line);
                }
            //})).Start();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var qa in Answers)
            {
                if (qa == null)
                {
                    sb.Append("NULL");
                }
                else
                {
                    sb.Append(qa.ToString());
                }
                
            }
            return $"QuestDialog {GetHashCode()} [{Id}/Answers:{Answers.Count} [{sb.ToString()}]]";;
        }

        [HideInInspector]
        public string GUID {  get { return Guid; } set { Guid = value; } }

        [HideInInspector]
        [SerializeField] 
        public string Guid;
        
        public void GetLinks(HashSet<Linkable> links)
        {
            links.Add(VisibilityCombinator);
            foreach (var answer in Answers)
            {
                links.Add(answer);
            }
        }

        public void RestoreLinks(ReferenceSerializer dictionary)
        {
            Answers.Clear();

            VisibilityCombinator = dictionary.GetObject<CombinatorData>(VisibilityCombinatorGuid);
            foreach (var guid in AnswersGUIDS)
            {
                Answers.Add(dictionary.GetObject<QuestAnswer>(guid));
            }
        }

        public void StoreLinks()
        {
            AnswersGUIDS.Clear();
            VisibilityCombinatorGuid = VisibilityCombinator?.GUID;
            foreach (var answer in Answers)
            {
                AnswersGUIDS.Add(answer.GUID);
            }
        }
    }
}