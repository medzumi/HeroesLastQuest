using System;
using System.Collections;
using Fishing;
using UnityEngine;

[Serializable]
public class ThrowingState : AbstractState
{
    private enum ThrowingStates
    {
        Flying,
        Earth,
        Water,
        Error
    }
    
    [SerializeField] private string _nextStateKey;
    [SerializeField] private string _resetStateKey;

    [SerializeField] private GameObject _aim;
    [SerializeField] private Rigidbody _swimmer;
    [SerializeField] private GameObjectData _gameObjectData;

    [SerializeField] private FisherData _fisherData;

    private int _layerId;
    private ThrowingStates _state;

    private Coroutine _lengthCoroutine;
    public override string Update()
    {
        switch (_state)
        {
            case ThrowingStates.Water :
                _swimmer.constraints = RigidbodyConstraints.FreezeAll;
                _swimmer.rotation = Quaternion.identity;
                return _nextStateKey;
            case ThrowingStates.Error:
            case ThrowingStates.Earth:
                return _resetStateKey;
        }
        return Key;
    }

    public override void Enter()
    {
        _swimmer.constraints = RigidbodyConstraints.None;
        _layerId = LayerMask.NameToLayer("Water");
        _swimmer.gameObject.SetActive(true);
        _swimmer.gameObject.GetOrCreateComponent<CollisionEnterHandler>()
            .OnTrigger += TriggerEnterHandler;
        _swimmer.gameObject.GetOrCreateComponent<TriggerExitHandler>()
            .OnTrigger += TriggerExitHandler;
        _state = ThrowingStates.Flying;
        var forcePercentage = _gameObjectData.ReadData<float>("ThrowForce");
        _swimmer.position = _aim.transform.position;
        _swimmer.AddForce(_aim.transform.forward * (_fisherData.MinThrowForce + (_fisherData.MaxThrowForce - _fisherData.MinThrowForce) * forcePercentage));
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

    public override void Exit()
    {
        _swimmer.gameObject.GetOrCreateComponent<CollisionEnterHandler>()
            .OnTrigger -= TriggerEnterHandler;
        _swimmer.gameObject.GetOrCreateComponent<TriggerExitHandler>()
            .OnTrigger -= TriggerExitHandler;
    }
}

[Serializable]
public class DebugState : IState
{
    [SerializeField] private string _key;
    [SerializeField] private string _nextKey;

    public string Key => _key;
    public string Update()
    {
        return _nextKey;
    }

    public void Enter()
    {
        Debug.LogError($"Enter to {_key}");
    }

    public void Exit()
    {
        Debug.LogError($"Exit from {_key} to {_nextKey}");
    }
}