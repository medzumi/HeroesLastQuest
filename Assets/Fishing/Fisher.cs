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
    [SerializeField] private CostylState _costylState;

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
            _upState,
            _costylState
        };
        stateList.AddRange(_animationStates);
        stateList.AddRange(_debugStates);
        _stateMachine = new StateMachine(_fishingIdleState, stateList);
    }

    public void SetState(string stateId)
    {
        _stateMachine.SetState(stateId);
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