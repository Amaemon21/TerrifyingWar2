public class DieState : State
{
    public DieState(Enemy enemy) : base(enemy) { }
    
    public override void Enter()
    {
        Enemy.StopMovement();
    }

    public override void Update()
    {
    }

    public override void Exit()
    {
    }
}