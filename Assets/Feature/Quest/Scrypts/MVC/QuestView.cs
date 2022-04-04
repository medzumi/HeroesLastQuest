using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Quest.Scrypts.MVC
{
    public class QuestView : MonoBehaviour
    {
        private QuestController _questController;

        public static bool IsEnabledInput = false;
        
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
        
        [SerializeField]
        private Button questButton;

        public UnityEvent OnOpen;
        public UnityEvent OnClosed;

        private void Start()
        {
            _questController = QuestController.Singleton;
            _questController.OnChangeActive += ChangeActive;
            _questController.OnRepaintCompleted += RepaintCompleted;
            _questController.OnRepaintInProgress += RepaintInProgress;
            currentQuestView.OnClickButton += ClickQuest;
            closeButton.onClick.AddListener(Close);
            questButton.onClick.AddListener(OpenQuestClick);
            _questController.OpenFirstTest();
        }


        private void OnDestroy()
        {
            _questController.OnChangeActive -= ChangeActive;
            _questController.OnRepaintCompleted -= RepaintCompleted;
            _questController.OnRepaintInProgress -= RepaintInProgress;
            currentQuestView.OnClickButton -= ClickQuest;
            questButton.onClick.RemoveListener(OpenQuestClick);
            closeButton.onClick.RemoveListener(Close);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                OpenQuestClick();
            }
        }

        private void OpenQuestClick()
        {
            ChangeActive(true, true);
        }

        private void Close()
        {
            panel.SetActive(false);
            IsEnabledInput = true;
            OnClosed?.Invoke();   
            //SceneManager.LoadScene (sceneName: "Scenes/Fishing");
        }

        private void ClickQuest(QuestViewParameters obj)
        {
            questButton.gameObject.SetActive(true);
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
            if (isActive)
            {
                OnOpen?.Invoke();
            }
            else
            {
                OnClosed?.Invoke();
            }

            IsEnabledInput = !isActive;
            closeButton.gameObject.SetActive(isCanClose);
        }
    }

    public class QuestViewParameters
    {
        public Configs.Quest QuestConfig;
        public float Value;
    }
}