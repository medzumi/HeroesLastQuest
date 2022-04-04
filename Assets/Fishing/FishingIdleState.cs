using System;
using UnityEngine;

[Serializable]
public class FishingIdleState : IState
{
    [SerializeField] private string _key;
    [SerializeField] private string _nextStateKey;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animatorIdleKey;
    [SerializeField] private Rigidbody _swimmer;
    [SerializeField] Transform _aim;
    public string Key => _key;
    
    public string Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            return _nextStateKey;
        }
        else
        {
            return _key;
        }
    }

    public void Enter()
    {
        _swimmer.constraints = RigidbodyConstraints.FreezePositionY;
        _swimmer.position = _aim.position;
        _swimmer.rotation = _aim.rotation;
        _animator.SetBool(_animatorIdleKey, true);
    }

    public void Exit()
    {
        _animator.SetBool(_animatorIdleKey, false);
    }
}