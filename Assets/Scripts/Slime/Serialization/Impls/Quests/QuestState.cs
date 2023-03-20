using System;
using System.Collections.Generic;

namespace ROR.Core.Serialization
{
    [Serializable]
    public class QuestState
    {
        public string QuestId;
        public bool IsTaken;
        public bool IsFinished;
        public bool IsFailed;
        public Dictionary<string, string> Data = new Dictionary<string, string>();
    }
}