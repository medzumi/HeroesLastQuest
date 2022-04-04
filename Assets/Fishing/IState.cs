public interface IState
{
    string Key { get; }
    string Update();

    void Enter();
    void Exit();
}