using System.Threading;
using UnityEngine;

public class ReaperAttackState : IState
{
    private Reaper boss;

    public ReaperAttackState(Reaper boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        if (boss.IsDead)
        {
            boss.StateMachine.ChangeState(new ReaperDeadState(boss));
            return;
        }

        // Attack 상태 진입
        boss.Attack();
    }

    public void Exit()
    {
        // Attack 상태 해제
    }

    public void Update()
    {
        
    }
}
