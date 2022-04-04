using UnityEngine;

namespace Fishing
{
    [CreateAssetMenu]
    public class FisherData : ScriptableObject
    {
        public float ThrowSpeed;
        public float MinThrowForce;
        public float MaxThrowForce;
        public float NormalSpeed;
        public float MinNibble = 1;
        public float MaxNibble = 10;
        public float MinTug = 1;
        public float MaxTug = 3;
        public float TUnderwater;
        public float MinDistance;
        public float MaxDistance;
        public float TRotate;
        public float FishingSpeed;
        public float FishSpeed;
        public float MinHold;
        public float MaxHold;
        public float PlusChance;
        public float StartChance;
        public float LStart;
        public float FinishAnimationSpeed;
    }
}