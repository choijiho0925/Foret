using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// 할아버지(원본이미지)는 점점 하얘져야 됨
/// 하드라이트 색상은 저기서 바꾸고 싶지 않음
/// 근데 할아버지가 하얘지니까 하드라이트는 실시간 계산하니까 점점 하얘짐
/// 그래서 하드라이트를 고정하고 싶음

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

        // 텔레포트 준비 애니메이션 재생
        boss.PlayTeleportReadyAnimation();
    }

    public void Exit() 
    {
        isTeleportStarted = false;  // 상태 종료 시에도 초기화
    }

    public void Update() { }

    // 애니메이션 이벤트에서 호출됨
    public void StartTeleport()
    {
        boss.ResetAllAnimation();
        isTeleportStarted = true;
        boss.StartCoroutine(TeleportAttack());
    }

    private IEnumerator TeleportAttack()
    {
        // 플레이어 위쪽으로 offset
        Vector2 playerAbove = boss.Player.transform.position + Vector3.up * 5f;

        // 시도
        if (boss.CanTeleportTo(playerAbove))
        {
            // 텔레포트 선딜
            yield return new WaitForSeconds(0.7f);

            // 실제 텔레포트
            boss.ApplyTeleportRotation();
            boss.TeleportTo(playerAbove);

            // 텔레포트 성공 후 낙하 공격
            boss.PlayAttackAnimation();

            // 텔레포트 후 약간의 딜레이
            // yield return new WaitForSeconds(0.2f);

            // 공격 애니메이션 실행
            //boss.PlayAttackAnimation();
        }

        boss.TryChangeState(new FGDecisionState(boss));
    }
}
