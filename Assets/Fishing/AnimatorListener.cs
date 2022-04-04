using System;
using UnityEngine;

public partial class AnimatorListener : MonoBehaviour
{
    public event Action<string> OnAnimatorFire;

    void FireAnimatorKey(string key)
    {
        OnAnimatorFire?.Invoke(key);
    }
}