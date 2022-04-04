using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quest.Scrypts.Configs
{
    [CreateAssetMenu(menuName = "Quest/QuestAsset")]
    public class QuestAsset : ScriptableObject
    {
        public List<Quest> InProgressQuest;
        public List<Quest> Completed;
    }

    [Serializable]
    public class Quest
    {
        
        public string NameQuest;
        public int IdQuest;
        public string DescriptionQuest;
        public string SecondDescriptionQuest;
        public string ButtonText;
        public string QuestEndeText;
        public float ValueQuest;
        public TypeValueQuest TypeValueQuest;

    }

    public enum TypeValueQuest
    {
        Percent,
        Count
    }
}