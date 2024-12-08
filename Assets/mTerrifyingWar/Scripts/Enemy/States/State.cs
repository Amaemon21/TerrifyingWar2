public abstract class State
{
    protected Enemy Enemy;

    protected State(Enemy enemy)
    {
        Enemy = enemy;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}