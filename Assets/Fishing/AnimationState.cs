using System;
using Fishing;
using UnityEngine;

[Serializable]
public class AnimationState : AbstractState
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _endAnimationKey;
    [SerializeField] private string _nextKey;

    private string _returnableKey;
    public override string Update()
    {
        return _returnableKey;
    }

    public override void Enter()
    {
        _returnableKey = Key;
        _animator
            .gameObject
            .GetOrCreateComponent<AnimatorListener>()
            .OnAnimatorFire += AnimatorFireHandler;
    }

    private void AnimatorFireHandler(string obj)
    {
        if (string.Equals(obj, _endAnimationKey))
        {
            _returnableKey = _nextKey;
        }
    }

    public override void Exit()
    {
        _animator
            .gameObject
            .GetOrCreateComponent<AnimatorListener>()
            .OnAnimatorFire -= AnimatorFireHandler;
    }
}