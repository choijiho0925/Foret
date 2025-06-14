using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어에게 근접 공격을 가하는 상태 클래스
/// </summary>
public class FGMeleeState : IState
{
    private ForestGuardian boss;

    public FGMeleeState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.ResetAllAnimation();
        boss.StartCoroutine(MeleeAttack());
        Debug.Log("FGMeleeState 진입");
    }

    public void Exit() { }

    public void Update() { }

    private IEnumerator MeleeAttack()
    {
        while (true)
        {
            // 플레이어가 근접 범위에 있는지 확인
            if (boss.GetPlayerDistance() > boss.AttackRange)
            {
                yield return new WaitForSeconds(boss.patternDelay);
                boss.StateMachine.ChangeState(new FGDecisionState(boss));
                yield break;
            }

            // 공격 애니메이션
            boss.PlayeAttackAnimation();

            // 공격
            boss.Attack();

            // 애니메이션 완료까지 대기
            yield return new WaitForSeconds(boss.AttackDuration);

            
        }
    }
}
