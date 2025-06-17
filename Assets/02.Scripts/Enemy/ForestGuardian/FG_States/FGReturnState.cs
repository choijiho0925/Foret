using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 숲의 주인 보스를 제자리로 돌려보내는 상태 클래스
/// </summary>
public class FGReturnState : IState
{
    public ForestGuardian boss;

    public FGReturnState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.ResetAllAnimation();  
    }

    public void Exit()
    {

    }

    public void Update()
    {
        float playerDistance = boss.GetPlayerDistance();
        float returnDistance = Vector3.Distance(boss.transform.position, boss.InitialPosition);

        // 플레이어가 가까우면 공격 상태로 전이
        if (playerDistance < boss.BackdownRange)
        {
            boss.StateMachine.ChangeState(new FGMeleeState(boss));
            return;
        }

        // 아직 제자리에 도착하지 않았으면 이동
        if (returnDistance > boss.DetectionRange)
        {
            // 제자리로 이동
            boss.transform.position = boss.InitialPosition;
        }

        else
        {
            // 제자리에 도착했으면 다음 상태로 전이
            boss.StateMachine.ChangeState(new FGDecisionState(boss));
        }
    }
}
