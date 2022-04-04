using System;
using Fishing;
using UnityEngine;

[Serializable]
public class FishingIdleState : AbstractState
{
    [SerializeField] private string _nextStateKey;
    [SerializeField] private Animator _animator;
    [SerializeField] private string _animatorIdleKey;
    [SerializeField] private Rigidbody _swimmer;
    [SerializeField] private Transform _rod;
    [SerializeField] Transform _aim;
    [SerializeField] private Transform _defaultRodRot;

    public override string Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            return _nextStateKey;
        }
        else
        {
            return Key;
        }
    }

    public override void EnterHandler()
    {
        _rod.rotation = _defaultRodRot.rotation;
        _swimmer.constraints = RigidbodyConstraints.FreezePositionY;
        _swimmer.transform.position = _aim.position;
        _swimmer.transform.rotation = _aim.rotation;
        _animator.SetBool(_animatorIdleKey, true);
    }

    public override void ExitHandler()
    {
        _animator.SetBool(_animatorIdleKey, false);
    }
}