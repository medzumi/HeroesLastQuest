using UnityEngine;

public static class UnityExtensions
{
    public static T GetOrCreateComponent<T>(this GameObject gameObject)
        where T : Component
    {
        if (gameObject.TryGetComponent(out T component))
        {
            return component;
        }
        else
        {
            return gameObject.AddComponent<T>();
        }
    }
}