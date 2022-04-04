using System;
using UnityEngine;
using UnityEngine.UI;

namespace Feature.QuestPopup
{
    public class QuestPopupView : MonoBehaviour
    {
        private QuestPopupController _questPopupController;

        [SerializeField]
        private GameObject panel;

        [SerializeField]
        private Text textPanel;

        [SerializeField]
        private float timePopup;
        [SerializeField]
        private float deltaTime;

        private float _time1;
        
        private float _time2;

        private bool _isActive1;
        
        private bool _isActive2;

        private void Start()
        {
            _questPopupController = QuestPopupController.Singleton;
            _questPopupController.OnQuestEnded += QuestEnded;
        }
        
        private void OnDisable()
        {
            _questPopupController.OnQuestEnded -= QuestEnded;
        }

        private void Update()
        {
            if (_isActive1)
            {
                _time1 -= Time.deltaTime;
                if (_time1 <= 0)
                {
                    panel.SetActive(true);
                    _isActive1 = false;
                    _isActive2 = true;
                }
            }

            if (_isActive2)
            {
                _time2 -= Time.deltaTime;
                if (_time2<=0)
                {
                    _isActive2 = false;
                    panel.SetActive(false);
                }
            }
        }

        private void QuestEnded(Quest.Scrypts.Configs.Quest obj)
        {
           
            textPanel.text = obj.QuestEndeText;
            _time1 = deltaTime;
            _time2 = timePopup;
            _isActive1 = true;
        }
    }
}