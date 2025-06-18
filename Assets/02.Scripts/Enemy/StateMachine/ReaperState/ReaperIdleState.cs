using UnityEngine;

public class ReaperIdleState : IState
{
    private Reaper boss;

    public ReaperIdleState(Reaper boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // Idle 상태 진입
    }

    public void Exit()
    {
        // Idle 상태 해제
    }

    public void Update()
    {
        if (boss.IsDead)
        {
            boss.StateMachine.ChangeState(new ReaperDeadState(boss));
            return;
        }

        float distance = Vector3.Distance(boss.transform.position, boss.Player.transform.position);

        if (distance >= boss.DetectionRange) return;

        if (distance > boss.AttackRange)
            boss.StateMachine.ChangeState(new ReaperChaseState(boss));
        else
            boss.StateMachine.ChangeState(new ReaperAttackState(boss));
    }
}
