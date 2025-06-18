using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어에게 근접 공격을 가하는 상태 클래스
/// </summary>
public class FGMeleeState : IState
{
    private ForestGuardian boss;
    private Coroutine meleeRoutine;

    public FGMeleeState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.ResetAllAnimation();
        meleeRoutine = boss.StartCoroutine(MeleeAttack());
    }

    public void Exit() 
    {
        if (meleeRoutine != null)
        {
            boss.StopCoroutine(meleeRoutine); // 안전하게 중단
            meleeRoutine = null;
        }
    }

    public void Update() { }

    private IEnumerator MeleeAttack()
    {
        while (true)
        {
            float distance = boss.GetPlayerDistance();

            // 플레이어가 죽었으면 공격 멈춤
            if (GameManager.Instance.player.isDead)
            {
                boss.StateMachine.ChangeState(new FGIdleState(boss));
                yield break;
            }

            // 너무 가까워서 회피 상태로 전환
            if (distance < boss.BackdownRange)
            {
                boss.StateMachine.ChangeState(new FGBackdownState(boss));
                yield break;
            }

            // 플레이어가 근접 범위에 있는지 확인
            if (boss.GetPlayerDistance() > boss.AttackRange)
            {
                yield return new WaitForSeconds(boss.patternDelay);
                boss.StateMachine.ChangeState(new FGDecisionState(boss));
                yield break;
            }

            // 공격 애니메이션
            boss.PlayAttackAnimation();

            // 공격
            boss.Attack();

            // 애니메이션 완료까지 대기
            yield return new WaitForSeconds(boss.AttackDuration);
        }
    }
}
