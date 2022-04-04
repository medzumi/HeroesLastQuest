using System.Collections.Generic;

public class StateMachine
{
    private readonly IState _startState;
    private IState _currentState;
    private readonly Dictionary<string, IState> _states;

    public StateMachine(IState startState, IEnumerable<IState> states)
    {
        _startState = startState;
        _states = new Dictionary<string, IState>();
        foreach (var state in states)
        {
            _states[state.Key] = state;
        }
        _currentState = _startState;
        _currentState.Enter();
    }

    public void Update()
    {
        SetState(_currentState.Update());
    }

    public void SetState(string key)
    {
        if (_states.TryGetValue(key, out var state))
        {
            if (!object.ReferenceEquals(state, _currentState))
            {
                _currentState.Exit();
                _currentState = state;
                _currentState.Enter();
            }
        }
    }

    public void Reset()
    {
        _currentState = _startState;
    }
}