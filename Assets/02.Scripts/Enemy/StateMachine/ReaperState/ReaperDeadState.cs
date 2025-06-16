using System.Threading;
using UnityEngine;

public class ReaperDeadState : IState
{
    private Reaper boss;

    public ReaperDeadState(Reaper boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // Dead 상태 진입
        boss.AnimationHandler.Dead();
        boss.Die();
    }

    public void Exit()
    {
        // Dead 상태 해제
    }

    public void Update()
    {
        
    }
}
