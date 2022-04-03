using System;
using UnityEngine;

namespace Feature.QuestPopup
{
    public class QuestPopupController: MonoBehaviour
    {
        public static QuestPopupController Singleton;

        public event Action<Quest.Scrypts.Configs.Quest> OnQuestEnded;

        private void Awake()
        {
            Singleton = this;
        }

        public void QuestEnded(Quest.Scrypts.Configs.Quest quest)
        {
            OnQuestEnded?.Invoke(quest);
        }
    }
}