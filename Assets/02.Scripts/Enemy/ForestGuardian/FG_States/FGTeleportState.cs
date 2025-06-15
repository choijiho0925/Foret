using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 위로 텔레포트해 하단 공격을 가하는 상태 클래스
/// </summary>
public class FGTeleportState : IState
{
    private ForestGuardian boss;
    private bool isTeleportStarted = false;

    public FGTeleportState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.ResetAllAnimation();
        isTeleportStarted = false;  // 상태 재진입 시 플래그 초기화
        boss.PlayAttackAnimation();
        Debug.Log("FGTeleportState 진입");
    }

    public void Exit() 
    {
        isTeleportStarted = false;  // 상태 종료 시에도 초기화
    }

    public void Update() { }

    // 애니메이션 이벤트에서 호출됨
    public void StartTeleport()
    {
        Debug.Log("StartTeleport 시도");

        boss.ResetAllAnimation();
        isTeleportStarted = true;
        boss.StartCoroutine(TeleportAttack());
    }

    private IEnumerator TeleportAttack()
    {
        // 플레이어 위쪽으로 offset
        Vector2 playerAbove = boss.Player.transform.position + Vector3.up * 5f;

        // 위치 확인용 디버그
        Debug.Log($"텔레포트 시도 위치: {playerAbove}");

        // 시도
        if (boss.CanTeleportTo(playerAbove))
        {
            // 텔레포트 선딜
            yield return new WaitForSeconds(0.3f);

            // 실제 텔레포트
            boss.ApplyTeleportRotation();
            boss.TeleportTo(playerAbove);
            Debug.Log("▶ 순간이동 성공");

            // 텔레포트 후 약간의 딜레이
            // yield return new WaitForSeconds(0.2f);

            // 공격 애니메이션 실행
            //boss.PlayAttackAnimation();
        }
        else
        {
            Debug.Log("✖ 순간이동 실패 - 충돌 또는 공간 없음");
        }

        boss.TryChangeState(new FGDecisionState(boss));
    }
}
