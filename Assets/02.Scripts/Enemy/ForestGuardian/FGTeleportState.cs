using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 위로 텔레포트해 하단 공격을 가하는 상태 클래스
/// </summary>
public class FGTeleportState : IState
{
    private ForestGuardian boss;

    public FGTeleportState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.ResetAllAnimation();
        boss.StartCoroutine(TeleportAttack());
        Debug.Log("FGTeleportState 진입");
    }

    public void Exit() { }

    public void Update() { }

    private IEnumerator TeleportAttack()
    {
        // 플레이어 위쪽으로 offset
        Vector2 playerAbove = boss.Player.transform.position + Vector3.up * 5f;

        // 위치 확인용 디버그
        Debug.Log($"텔레포트 시도 위치: {playerAbove}");

        // 시도
        if (boss.CanTeleportTo(playerAbove))
        {
            boss.TeleportTo(playerAbove);
            Debug.Log("▶ 순간이동 성공");
        }
        else
        {
            Debug.Log("✖ 순간이동 실패 - 충돌 또는 공간 없음");
        }

        yield return new WaitForSeconds(boss.patternDelay);

        boss.StateMachine.ChangeState(new FGDecisionState(boss));
    }
}
