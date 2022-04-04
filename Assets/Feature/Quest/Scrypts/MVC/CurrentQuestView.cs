using System;
using Quest.Scrypts.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Quest.Scrypts.MVC
{
    public class CurrentQuestView : MonoBehaviour
    {
        [SerializeField]
        private Text nameQuest;
        [SerializeField]
        private Text descriptionQuest;
        [SerializeField]
        private Text secondDescriptionQuest;
        
        [SerializeField]
        private Text buttonText;

        [SerializeField]
        private Button buttonQuest;

        public event Action<QuestViewParameters> OnClickButton;

        private QuestViewParameters _parameters;

        private void OnEnable()
        {
            buttonQuest.onClick.AddListener(ClickButton);
        }
        
        private void OnDisable()
        {
            buttonQuest.onClick.RemoveListener(ClickButton);
        }

        private void ClickButton()
        {
            OnClickButton?.Invoke(_parameters);
        }

        public void Initialize(QuestViewParameters parameters,bool isActive)
        {
            _parameters = parameters;
            nameQuest.text = parameters.QuestConfig.NameQuest;
            descriptionQuest.text = parameters.QuestConfig.DescriptionQuest;
            string valueQuest = "";
            buttonQuest.gameObject.SetActive(isActive);
            buttonText.text = parameters.QuestConfig.ButtonText;
            if (parameters.QuestConfig.TypeValueQuest == TypeValueQuest.Percent)
            {
                if (isActive)
                {
                   
                    valueQuest = $"{parameters.Value}%/100%";
                }
                else
                {
                    valueQuest = "100%";  
                }
            }
            else
            {
                valueQuest = $"{parameters.Value}/{parameters.QuestConfig.ValueQuest}";
            }
            
            secondDescriptionQuest.text = parameters.QuestConfig.SecondDescriptionQuest +" "+ valueQuest;


        }
    }
}