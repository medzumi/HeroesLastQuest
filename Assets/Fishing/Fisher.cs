using System;
using System.Collections.Generic;
using UnityEngine;

public class Fisher : MonoBehaviour
{
    private StateMachine _stateMachine;

    [SerializeField] private FishingIdleState _fishingIdleState;
    [SerializeField] private AimingState _aimingState;
    [SerializeField] private ThrowingState _throwingState;
    [SerializeField] private List<AnimationState> _animationStates;

    private void Awake()
    {
        var stateList = new List<IState>()
        {
            _fishingIdleState,
            _aimingState,
            _throwingState,
        };
        stateList.AddRange(_animationStates);
        _stateMachine = new StateMachine(_fishingIdleState, stateList);
    }

    private void OnEnable()
    {
        _stateMachine.Reset();
    }

    private void Update()
    {
        _stateMachine.Update();
    }
    
    public partial class AnimatorListener : MonoBehaviour
    {
        public event Action<string> OnAnimatorFire;

        void FireAnimatorKey(string key)
        {
            OnAnimatorFire?.Invoke(key);
        }
    }
    
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
}