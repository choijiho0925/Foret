using System.Collections;
using UnityEngine;

public class ReaperDamageState : IState
{
    private Reaper boss;

    public ReaperDamageState(Reaper boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // Damage 상태 진입
        boss.StartCoroutine(Invincible());
    }

    public void Exit()
    {
        // Damage 상태 해제
    }

    public void Update()
    {
        
    }

    private IEnumerator Invincible()
    {
        boss.IsInvincible = true;
        yield return new WaitForSeconds(0.5f);
        boss.IsInvincible = false;

        boss.StateMachine.ChangeState(new ReaperIdleState(boss));
    }
}
