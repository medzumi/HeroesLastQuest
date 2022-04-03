using System;
using UnityEngine;
using UnityEngine.UI;

namespace Quest.Scrypts.MVC
{
    public class FishPopupView : MonoBehaviour
    {
        
        private FishPopupController _fishPopupController;

        [SerializeField]
        private GameObject panel;

        [SerializeField] 
        private Text nameFish;

        [SerializeField]
        private Transform contentStar;
        
        [SerializeField] 
        private GameObject starPrefab;

        [SerializeField] 
        private float deltaTime;
        
         
        private float _time;
        private bool _isActive;
        
        private void Start()
        {
            _fishPopupController = FishPopupController.Singleton;
            _fishPopupController.OnAddFish += AddFish;
        }

        private void Update()
        {
            if (_isActive)
            {
                _time -= Time.deltaTime;
                if (_time <= 0)
                {
                    _isActive = false;
                    panel.SetActive(false);
                }
            }

            
        }


        private void OnDisable()
        {
            _fishPopupController.OnAddFish -= AddFish;
        }

        private void AddFish(FishConfig config)
        {
            panel.SetActive(true);
            nameFish.text = config.NameFish;

            for (int i = 0; i < contentStar.childCount; i++)
            {
                Destroy(contentStar.GetChild(i).gameObject);
            }

            for (int i = 0; i < config.StarFish; i++)
            {
                Instantiate(starPrefab, contentStar);
            }

            
            _time = deltaTime;
            _isActive = true;
        }


    }
}