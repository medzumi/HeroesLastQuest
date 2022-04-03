using UnityEngine;

public class Fisher : MonoBehaviour
{
    private StateMachine _stateMachine;

    [SerializeField] private FishingIdleState _fishingIdleState;
    [SerializeField] private AimingState _aimingState;
    [SerializeField] private ThrowingState _throwingState;

    private void Awake()
    {
        _stateMachine = new StateMachine(_fishingIdleState, new IState[]
        {
            _fishingIdleState,
            _aimingState,
            _throwingState
        });
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