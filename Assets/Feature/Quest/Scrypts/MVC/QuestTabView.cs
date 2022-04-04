using System;
using UnityEngine;
using UnityEngine.UI;

namespace Quest.Scrypts.MVC
{
    public class QuestTabView : MonoBehaviour
    {
        [SerializeField]
        private Text nameQuestText;

        [SerializeField]
        private Button buttonTab;

        public event Action<QuestViewParameters> OnClickTab;

        private QuestViewParameters _parameters;
        
        private void OnEnable()
        {
            buttonTab.onClick.AddListener(ClickTab);
        }

        private void OnDisable()
        {
            buttonTab.onClick.RemoveListener(ClickTab);
        }

        private void ClickTab()
        {
            OnClickTab?.Invoke(_parameters);
        }

        public void Initialize(QuestViewParameters parameters)
        {
            _parameters = parameters;
            nameQuestText.text = parameters.QuestConfig.NameQuest;
        }
    }
}