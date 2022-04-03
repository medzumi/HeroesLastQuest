using System;
using UnityEngine;

public class TriggerExitHandler : MonoBehaviour
{
    public event Action<GameObject> OnTrigger;

    private void OnTriggerExit(Collider other)
    {
        OnTrigger?.Invoke(other.gameObject);
    }
}