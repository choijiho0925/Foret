using System.Collections;
using UnityEngine;

public class ReaperTeleportState : IState
{
    private Reaper boss;

    public ReaperTeleportState(Reaper boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // Teleport 상태 진입
        boss.StartCoroutine(TeleportState());
    }

    public void Exit()
    {
        // Teleport 상태 해제
    }

    public void Update()
    {

    }

    private IEnumerator TeleportState()
    {
        yield return boss.TeleportAttack();

        // 텔포 공격 종료 후 Idle 상태로 전환
        boss.StateMachine.ChangeState(new ReaperIdleState(boss));
    }
}
