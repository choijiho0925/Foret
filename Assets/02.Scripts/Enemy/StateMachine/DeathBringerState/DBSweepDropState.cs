using System.Collections;
using UnityEngine;

public class DBSweepDropState : IState
{
    private DeathBringer boss;

    public DBSweepDropState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.StartCoroutine(SweepAndDropState());
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }

    private IEnumerator SweepAndDropState()
    {
        boss.BossAnimationHandler.Attack2();
        yield return boss.SweepAndDrop();

        // 상태 종료 후 다음 상태로 전환
        boss.StateMachine.ChangeState(new DBIdleState(boss));
    }
}
