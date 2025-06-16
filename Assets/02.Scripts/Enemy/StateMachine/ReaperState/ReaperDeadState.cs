using UnityEngine;

public class ReaperDeadState : IState
{
    private BossBase boss;

    public ReaperDeadState(BossBase boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // Dead 상태 진입
    }

    public void Exit()
    {
        // Dead 상태 해제
    }

    public void Update()
    {
        
    }
}
