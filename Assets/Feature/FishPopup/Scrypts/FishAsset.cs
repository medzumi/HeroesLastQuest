using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quest.Scrypts.MVC
{
    [CreateAssetMenu(menuName = "Fish/FishAsset")]
    public class FishAsset : ScriptableObject
    {
        
        public List<FishConfig> listFish;
    }

    [Serializable]
    public class FishConfig
    {
        public string NameFish;
        public int FishId;
        public int StarFish;
        public float Rare;
    }
}