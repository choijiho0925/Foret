using System.Threading;
using UnityEngine;

public class ReaperAttackState : IState
{
    private BossBase boss;

    public ReaperAttackState(BossBase boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
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
