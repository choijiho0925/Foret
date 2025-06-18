using System.Collections;
using UnityEngine;

public class ReaperSummonMinionsState : IState
{
    private Reaper boss;

    public ReaperSummonMinionsState(Reaper boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // SummonMinions 상태 진입
        boss.BossAnimationHandler.Pattern2();
        boss.StartCoroutine(SummonMinionsState());
    }

    public void Exit()
    {
        // SummonMinions 상태 해제
    }

    public void Update()
    {

    }

    private IEnumerator SummonMinionsState()
    {
        yield return boss.SummonMinions();

        // 상태 종료 후 다음 상태로 전환
        boss.StateMachine.ChangeState(new ReaperIdleState(boss));
    }
}
