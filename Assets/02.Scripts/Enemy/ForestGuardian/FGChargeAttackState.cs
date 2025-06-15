using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어에게 돌진 공격을 가하는 상태 클래스
/// </summary>
public class FGChargeAttackState : IState
{
    private ForestGuardian boss;
    private bool isChargeStarted = false;

    public FGChargeAttackState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.ResetAllAnimation();
        boss.PlayChargeAnimation();
        Debug.Log("FGChargeAttackState 진입");
    }


    public void Exit() { }

    public void Update() { }

    // 애니메이션 이벤트에서 호출됨
    public void StartCharge()
    {
        if(!isChargeStarted)
        {
            isChargeStarted = true;
            boss.ResetAllAnimation();
            boss.StartCoroutine(ChargeAttack());
        }
    }

    private IEnumerator ChargeAttack()
    {
        // 돌진
        Vector2 direction = (boss.Player.transform.position - boss.transform.position).normalized;

        // 플레이어 조금 앞에서 멈춤
        float distanceOffset = 3f;
        Vector2 targetPos = (Vector2)boss.Player.transform.position - direction * distanceOffset;

        float chargeSpeed = boss.MoveSpeed * boss.ChargeSpeedMultiplier * 1.5f;      
       
        // 물리기반 이동
        Rigidbody2D rb = boss.GetComponent<Rigidbody2D>();

        // 이동 거리
        while (Vector2.Distance(rb.position, targetPos) > 0.05f)
        {
            Vector2 moveDelta = direction * chargeSpeed * Time.deltaTime;

            // 목표 지점 넘어서는 문제 방지
            if (Vector2.Distance(rb.position + moveDelta, targetPos) > Vector2.Distance(rb.position, targetPos))
                break;

            rb.MovePosition(rb.position + moveDelta);
            yield return null;
        }

        //// 색상 원상복귀
        //sprite.color = startColor;

        // 다음 상태로 전환
        yield return new WaitForSeconds(boss.patternDelay);
        boss.StateMachine.ChangeState(new FGDecisionState(boss));
    }
}
