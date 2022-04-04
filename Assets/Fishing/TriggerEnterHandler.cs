using System;
using UnityEngine;

public class TriggerEnterHandler : MonoBehaviour
{
    public event Action<GameObject> OnTrigger; 

    private void OnTriggerEnter(Collider other)
    {
        OnTrigger?.Invoke(other.gameObject);
    }
}

public class CollisionEnterHandler : MonoBehaviour
{
    public event Action<GameObject> OnTrigger;

    private void OnCollisionEnter(Collision collision)
    {
        OnTrigger?.Invoke(collision.gameObject);
    }
}