using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어를 쫓아가는 상태 클래스
/// </summary>
public class FGChaseState : IState
{
    private ForestGuardian boss;

    public FGChaseState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter() 
    { 
        Debug.Log("FGChaseState 진입");
        boss.ResetAllAnimation();
        boss.PlayRunAnimation();
    }

    public void Exit()
    {
        boss.ResetAllAnimation(); // 이동 중단 시 애니메이션도 정지
    }

    public void Update()
    {
        boss.MoveToPlayer(boss.Player.transform.position);

        float distance = boss.GetPlayerDistance();

        // 가까워졌으면 돌진 상태로
        if (distance <= boss.ChargeRange)
        {
            boss.StateMachine.ChangeState(new FGChargeAttackState(boss));
        }

        // 너무 멀면 복귀
        else if (distance > boss.DetectionRange)
        {
            boss.StateMachine.ChangeState(new FGReturnState(boss));
        }
    }
}
