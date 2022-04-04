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
    [SerializeField] private CatchState _catchState;
    [SerializeField] private UpState _upState;
    [SerializeField] private List<AnimationState> _animationStates;
    [SerializeField] private List<DebugState> _debugStates;

    private void Awake()
    {
        var stateList = new List<IState>()
        {
            _fishingIdleState,
            _aimingState,
            _throwingState,
            _fishingWaitState,
            _fishingState,
            _catchState,
            _upState
        };
        stateList.AddRange(_animationStates);
        stateList.AddRange(_debugStates);
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