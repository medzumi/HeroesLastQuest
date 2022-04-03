using System.Collections.Generic;
using UnityEngine;

public class GameObjectData : MonoBehaviour
{
    private class DataHandler<T>
    {
        public T Value;
    }
    
    private readonly Dictionary<string, object> _data = new Dictionary<string, object>();

    public void SetData<T>(string key, T data)
    {
        if (_data.TryGetValue(key, out var objData) && objData is DataHandler<T> dataHandler)
        {
            dataHandler.Value = data;
        }
        else
        {
            _data[key] = new DataHandler<T>()
            {
                Value = data
            };
        }
    }

    public T ReadData<T>(string key, T defaulValue = default)
    {
        if (_data.TryGetValue(key, out var objData) && objData is DataHandler<T> dataHandler)
        {
            return dataHandler.Value;
        }
        else
        {
            return defaulValue;
        }
    }
}