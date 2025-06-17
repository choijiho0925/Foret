using UnityEngine;

public class DBIdleState : IState
{
    private DeathBringer boss;

    public DBIdleState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        float distance = Vector3.Distance(boss.transform.position, boss.Player.transform.position);

        if (distance >= boss.DetectionRange) return;

        if (distance > boss.AttackRange)
            boss.StateMachine.ChangeState(new DBChaseState(boss));
        else
            boss.StateMachine.ChangeState(new DBAttackState(boss));
    }
}
