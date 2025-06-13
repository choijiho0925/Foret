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
        // 만약 플레이어와 가깝다면 바로 근접 공격 상태로 전환
        if(boss.GetPlayerDistance() < boss.BackdownRange)
        {
            boss.StateMachine.ChangeState(new FGMeleeState(boss));
        }    
    }

    public void Exit()
    {

    }

    public void Update()
    {
        // 상태 유지 중에도 플레이어 거리 체크해서 상태 전이
        float distance = boss.GetPlayerDistance();

        // 플레이어가 가까우면 근접 상태로
        if(distance < boss.BackdownRange)
        {
            boss.StateMachine.ChangeState(new FGMeleeState(boss));
        }

        // 초기 위치까지 거리 계산
        float returnDistance = Vector3.Distance(boss.transform.position, boss.InitialPosition);

        if(returnDistance > boss.ReturnThreshold)
        {
            // 제자리로 이동
            boss.MoveToPlayer(boss.InitialPosition);
        }

        else
        {
            // 도착 시 다음 상태로 전이
            Debug.Log("보스 초기 위치 도착");
            boss.StateMachine.ChangeState(new FGDecisionState(boss));
        }
    }
}
