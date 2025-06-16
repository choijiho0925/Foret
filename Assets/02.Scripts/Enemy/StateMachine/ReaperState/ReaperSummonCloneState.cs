using System.Collections;
using UnityEngine;

public class ReaperSummonCloneState : IState
{
    private Reaper boss;

    public ReaperSummonCloneState(Reaper boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // SummonClone 상태 진입
        boss.BossAnimationHandler.SummonClone();
        boss.StartCoroutine(SummonClonesState());
    }

    public void Exit()
    {
        // SummonClone 상태 해제
    }

    public void Update()
    {

    }

    private IEnumerator SummonClonesState()
    {
        yield return boss.SummonClones();

        // 상태 종료 후 다음 상태로 전환
        boss.StateMachine.ChangeState(new ReaperIdleState(boss));
    }
}
