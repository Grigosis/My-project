using System;
using ClassLibrary1.Logic;
using SecondCycleGame.Assets.Scripts.ObjectEditor;
using UnityEngine;

namespace ROR.Core.Serialization
{
    /// <summary>
    /// Может привязаться по Id к чему либо.
    /// Нужно для создания независимых слоёв квестов
    /// </summary>
    [Serializable]
    public class NPCAnswer : QuestAnswer
    {
        [ComboBoxEditor("QuestContext.Npcs")]
        [SerializeField] 
        public string Npc;
        
        [ComboBoxEditor("QuestContext.Npcs")]
        [SerializeField] 
        public string DialogId;
    }
}