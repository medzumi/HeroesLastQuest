using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Quest.Scrypts.MVC
{
    public class FishPopupController : MonoBehaviour
    {
        public static FishPopupController Singleton;
        
        public event Action<FishConfig> OnAddFish;

        [SerializeField]
        private FishAsset fishAsset;
        
        private void Awake()
        {
            Singleton = this;
        }
        
        public void AddFish(int id)
        {
            FishConfig config = null;
            foreach (var fish in fishAsset.listFish)
            {
                if (fish.FishId == id)
                {
                    config = fish;
                    continue; 
                }
            }
            QuestController.Singleton.AddFish(id);
            OnAddFish?.Invoke(config);
            
        }

        [ContextMenu("AddTestFish")]
        private void AddTestFish()
        {
            var id = Random.RandomRange(1, 6);
            AddFish(id);
        }

    }
}