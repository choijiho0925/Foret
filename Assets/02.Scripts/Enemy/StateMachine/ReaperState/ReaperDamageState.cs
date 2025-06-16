using UnityEngine;

public class ReaperDamageState : IState
{
    private BossBase boss;

    public ReaperDamageState(BossBase boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // Damage 상태 진입
    }

    public void Exit()
    {
        // Damage 상태 해제
    }

    public void Update()
    {
        
    }
}
