using System;
using UnityEngine;

[Serializable]
public class ThrowingState : IState
{
    private enum ThrowingStates
    {
        Flying,
        Earth,
        Water,
        Error
    }
    
    [SerializeField] private string _key;
    [SerializeField] private string _nextStateKey;
    [SerializeField] private string _resetStateKey;

    [SerializeField] private GameObject _aim;
    [SerializeField] private GameObject _swimmer;
    [SerializeField] private GameObjectData _gameObjectData;

    private int _layerId;
    private ThrowingStates _state;
    
    public string Key => _key;
    public string Update()
    {
        switch (_state)
        {
            case ThrowingStates.Water :
                return _nextStateKey;
            case ThrowingStates.Error:
            case ThrowingStates.Earth:
                return _resetStateKey;
        }
        return _key;
    }

    public void Enter()
    {
        _layerId = LayerMask.GetMask("Water");
        _swimmer.SetActive(true);
        _swimmer.GetOrCreateComponent<TriggerEnterHandler>()
            .OnTrigger += TriggerEnterHandler;
        _swimmer.GetOrCreateComponent<TriggerExitHandler>()
            .OnTrigger += TriggerExitHandler;
        _state = ThrowingStates.Flying;
    }

    private void TriggerExitHandler(GameObject obj)
    {
        
    }

    private void TriggerEnterHandler(GameObject obj)
    {
        if ((_layerId & obj.layer) > 0)
        {
            _state = ThrowingStates.Water;
        }
    }

    public void Exit()
    {
        _swimmer.GetOrCreateComponent<TriggerEnterHandler>()
            .OnTrigger -= TriggerEnterHandler;
        _swimmer.GetOrCreateComponent<TriggerExitHandler>()
            .OnTrigger -= TriggerExitHandler;
    }
}