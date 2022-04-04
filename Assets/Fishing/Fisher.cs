using System.Collections.Generic;
using Fishing;
using UnityEngine;

public class Fisher : MonoBehaviour
{
    private StateMachine _stateMachine;

    [SerializeField] private FishingIdleState _fishingIdleState;
    [SerializeField] private AimingState _aimingState;
    [SerializeField] private ThrowingState _throwingState;
    [SerializeField] private FishingWaitState _fishingWaitState;
    [SerializeField] private FishingState _fishingState;
    [SerializeField] private List<AnimationState> _animationStates;

    private void Awake()
    {
        var stateList = new List<IState>()
        {
            _fishingIdleState,
            _aimingState,
            _throwingState,
            _fishingWaitState,
            _fishingState
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
}