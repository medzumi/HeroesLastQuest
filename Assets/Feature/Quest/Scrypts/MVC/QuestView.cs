using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Quest.Scrypts.MVC
{
    public class QuestView : MonoBehaviour
    {
        private QuestController _questController;
        
        [SerializeField]
        private GameObject panel;
        
        [SerializeField]
        private Transform contentCompleted;
        
        [SerializeField]
        private Transform contentInProgress;

        [SerializeField]
        private QuestTabView questTabViewPrefab;

        [SerializeField]
        private CurrentQuestView currentQuestView;

        [SerializeField]
        private int startQuestId = 2;

        [SerializeField]
        private Button closeButton;

        private void Start()
        {
            _questController = QuestController.Singleton;
            _questController.OnChangeActive += ChangeActive;
            _questController.OnRepaintCompleted += RepaintCompleted;
            _questController.OnRepaintInProgress += RepaintInProgress;
            currentQuestView.OnClickButton += ClickQuest;
            closeButton.onClick.AddListener(Close);
            _questController.OpenFirstTest();
        }

        private void OnDisable()
        {
            _questController.OnChangeActive -= ChangeActive;
            _questController.OnRepaintCompleted -= RepaintCompleted;
            _questController.OnRepaintInProgress -= RepaintInProgress;
            currentQuestView.OnClickButton -= ClickQuest;
            closeButton.onClick.RemoveListener(Close);
        }

        private void Close()
        {
            panel.SetActive(false);
        }

        private void ClickQuest(QuestViewParameters obj)
        {
            _questController.ClickQuest(obj);
        }

        private void RepaintCompleted(List<QuestViewParameters> listParameters)
        {
            for (int i = 0; i < contentCompleted.childCount; i++)
            {
                var view = contentCompleted.GetChild(i);
                view.GetComponent<QuestTabView>().OnClickTab -= ClickTab;
                Destroy(view.gameObject);
            }
            
            foreach (var parameter in listParameters)
            {
                var view = Instantiate(questTabViewPrefab, contentCompleted);
                view.Initialize(parameter);
                view.OnClickTab += ClickTab;
            }
        }


        private void RepaintInProgress(List<QuestViewParameters> listParameters)
        {
            for (int i = 0; i < contentInProgress.childCount; i++)
            {
                var view = contentInProgress.GetChild(i);
                view.GetComponent<QuestTabView>().OnClickTab -= ClickTab;
                Destroy(view.gameObject);
            }
            
            foreach (var parameter in listParameters)
            {
                var view = Instantiate(questTabViewPrefab, contentInProgress);
                view.Initialize(parameter);
                view.OnClickTab += ClickTab;
                if (parameter.QuestConfig.IdQuest == startQuestId)
                {
                    ClickTab(parameter);
                }
            }
        }
        
        private void ClickTab(QuestViewParameters parameters)
        {
            var isActive = parameters.Value != parameters.QuestConfig.ValueQuest;
            currentQuestView.Initialize(parameters, isActive);
        }

        private void ChangeActive(bool isActive, bool isCanClose)
        {
            panel.SetActive(isActive);
            closeButton.gameObject.SetActive(isCanClose);
        }
    }

    public class QuestViewParameters
    {
        public Configs.Quest QuestConfig;
        public float Value;
    }
}