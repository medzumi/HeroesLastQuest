using System;
using UnityEngine;

[Serializable]
public class AnimationState : IState
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _endAnimationKey;
    [SerializeField] private string _nextKey;
    [SerializeField] private string _key;

    private string _returnableKey;
        
    public string Key => _key;
    public string Update()
    {
        return _returnableKey;
    }

    public void Enter()
    {
        _returnableKey = _key;
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

    public void Exit()
    {
        _animator
            .gameObject
            .GetOrCreateComponent<AnimatorListener>()
            .OnAnimatorFire -= AnimatorFireHandler;
    }
}