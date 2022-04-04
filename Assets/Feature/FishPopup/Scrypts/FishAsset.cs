﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quest.Scrypts.MVC
{
    [CreateAssetMenu(menuName = "Fish/FishAsset")]
    public class FishAsset : ScriptableObject
    {
        [Min(0)] public int LStart;
        [Min(0)] public float PlusChance;
        public List<FishConfig> listFish;
    }

    [Serializable]
    public class FishConfig
    {
        public string NameFish;
        public int FishId;
        public int StarFish;
        public float Rare;
        public float ChanceWeight;
        public float MinHold;
        public float MaxHold;
        public GameObject Prefab;
        public bool IsLegendary = false;
    }
}