

public interface IEnemyStates 
{

    EnemyState StateType { get; }

    void Enter();
    void Exit();
    void Tick();

}
