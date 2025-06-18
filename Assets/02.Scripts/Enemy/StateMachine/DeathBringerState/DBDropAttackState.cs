using System.Collections;
using UnityEngine;

public class DBDropAttackState : IState
{
    private DeathBringer boss;

    public DBDropAttackState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.BossAnimationHandler.Pattern1();
        boss.StartCoroutine(DropAttackState());
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }

    private IEnumerator DropAttackState()
    {
        yield return boss.DropAttacks();

        // 상태 종료 후 다음 상태로 전환
        boss.StateMachine.ChangeState(new DBIdleState(boss));
    }
}
