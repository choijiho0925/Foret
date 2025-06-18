using System.Collections;
using UnityEngine;

public class DBDamageState : IState
{
    private DeathBringer boss;

    public DBDamageState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.StartCoroutine(Invincible());
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }

    private IEnumerator Invincible()
    {
        boss.IsInvincible = true;
        yield return new WaitForSeconds(1f);
        boss.IsInvincible = false;

        boss.StateMachine.ChangeState(new DBIdleState(boss));
    }
}
