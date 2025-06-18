using UnityEngine;

public class DBAttackState : IState
{
    private DeathBringer boss;

    public DBAttackState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        if (boss.IsDead)
        {
            boss.StateMachine.ChangeState(new DBDeadState(boss));
            return;
        }
        boss.Attack();
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
}
