using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 숲의 주인 보스의 현재 상태를 판단하고, 적절한 상태로 전이시키는 상태 클래스
/// </summary>
public class FGDecisionState : IState
{
    private ForestGuardian boss;

    public FGDecisionState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        float distance = boss.GetPlayerDistance();

        Debug.Log($"[Decision] 거리: {distance}");

        if (distance < boss.BackdownRange)
        {
            // 회피
            Debug.Log("▶ Backdown");
            boss.StateMachine.ChangeState(new FGBackdownState(boss));
        }

        else if (distance <= boss.AttackRange)
        {
            // 근거리 공격
            Debug.Log("▶ Melee");
            boss.StateMachine.ChangeState(new FGMeleeState(boss));
        }

        else if (distance <= boss.ChargeRange)
        {
            // 돌진 공격
            Debug.Log("▶ ChargeAttack");
            boss.StateMachine.ChangeState(new FGChargeAttackState(boss));
        }

        else if (distance <= boss.TeleportRange)
        {
            // 추격
            Debug.Log("▶ Chase");
            boss.StateMachine.ChangeState(new FGChaseState(boss));
        }

        else if (distance <= boss.DetectionRange)
        {
            // 텔레포트 공격
            Debug.Log("▶ Teleport");
            boss.StateMachine.ChangeState(new FGTeleportState(boss));
        }
        else
        {
            // 복귀
            Debug.Log("▶ Return");
            boss.StateMachine.ChangeState(new FGReturnState(boss));
        }
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}
