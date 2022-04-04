using System;
using System.Collections.Generic;
using Feature.QuestPopup;
using Quest.Scrypts.Configs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Quest.Scrypts.MVC
{
    public class QuestController: MonoBehaviour
    {
        
        public static QuestController Singleton;
        
        [SerializeField]
        private QuestAsset questAsset;

        public event Action<List<QuestViewParameters>> OnRepaintCompleted;
        
        public event Action<List<QuestViewParameters>> OnRepaintInProgress;
        
        
        
        public event Action<bool, bool> OnChangeActive;
        
        [SerializeField]
        private float valueLastQuest;
        
        [SerializeField]
        private int questExitId;
        
        [SerializeField]
        private int questFishId;

        private float _currentFishValue = 0;
        
        [SerializeField]
        private int idEndFish = 4;

        private void Awake()
        {
            Singleton = this;
        }
       
        public void OpenQuest(bool isCanClose)
        {
            var listCompleted = new List<QuestViewParameters>();

            foreach (var quest in questAsset.Completed)
            {
                var parameters = new QuestViewParameters();
                parameters.QuestConfig = quest;
                parameters.Value = quest.ValueQuest;
                listCompleted.Add(parameters);
            }
            
            var listInProgress = new List<QuestViewParameters>();
            
            foreach (var quest in questAsset.InProgressQuest)
            {
                var parameters = new QuestViewParameters();
                if (quest.IdQuest == questExitId)
                {
                    parameters.QuestConfig = quest;
                    parameters.Value = valueLastQuest;
                }

                if (quest.IdQuest == questFishId)
                {
                    parameters.QuestConfig = quest;
                    parameters.Value = _currentFishValue;
                }
                
                listInProgress.Add(parameters);
            }
            OnChangeActive?.Invoke(true, isCanClose);
            OnRepaintCompleted?.Invoke(listCompleted);
            OnRepaintInProgress?.Invoke(listInProgress);
        }

        
        public void AddFish(int idFish)
        {
            if (idEndFish == idFish && _currentFishValue == 0)
            {
                _currentFishValue = 1;
                foreach (var quest in questAsset.InProgressQuest)
                {
                    if (quest.IdQuest == questFishId)
                    {
                        QuestPopupController.Singleton.QuestEnded(quest); 
                        continue;
                    }
                }

               

            }
        }

        public void ClickQuest(QuestViewParameters questViewParameters)
        {
            if (questViewParameters.QuestConfig.IdQuest == questExitId)
            {
                SceneManager.LoadScene (sceneName: "Scenes/Ending");
            }
            
            if (questViewParameters.QuestConfig.IdQuest == questFishId)
            {
                // OnChangeActive?.Invoke(false, false);
                SceneManager.LoadScene (sceneName: "Scenes/Fishing");
            }
            
        }
        [ContextMenu("OpenFirst")]
        public void OpenFirstTest()
        {
            OpenQuest(false);
        }
        
        [ContextMenu("OpenSecond")]
        private void OpenSecondTest()
        {
            OpenQuest(true);
        }
        

    }
}