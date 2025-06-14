using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어에게 돌진 공격을 가하는 상태 클래스
/// </summary>
public class FGChargeAttackState : IState
{
    private ForestGuardian boss;

    public FGChargeAttackState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.ResetAllAnimation();
        boss.StartCoroutine(ChargeAttack());
        Debug.Log("FGChargeAttackState 진입");
    }


    public void Exit() { }

    public void Update() { }

    private IEnumerator ChargeAttack()
    {
        // 플레이어를 바라보는 방향
        Vector2 direction = (boss.Player.transform.position - boss.transform.position).normalized;

        float chargeSpeed = boss.MoveSpeed * 3f;
        float chargeTime = 0.5f;

        float timer = 0f;

        // 추후 차지 방식 변경
        while(timer < chargeTime)
        {
            boss.transform.position += (Vector3)(direction * chargeSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(boss.patternDelay);
        boss.StateMachine.ChangeState(new FGDecisionState(boss));
    }
}
