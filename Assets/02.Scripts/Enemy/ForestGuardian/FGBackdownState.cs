using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어와의 거리를 재고 회피하는 상태 클래스
/// </summary>
public class FGBackdownState : IState
{
    private ForestGuardian boss;
    private float backDownDuration = 0.5f;
    private float timer;

    public FGBackdownState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter() 
    { 
        Debug.Log("FGBackdownState 진입");
        timer = 0f;
    }

    public void Exit() { }

    public void Update()
    {
        timer += Time.deltaTime;

        // 회피 실행
        boss.BackdownFromPlayer(boss.Player.transform.position);

        float distance = boss.GetPlayerDistance();

        if(distance > boss.BackdownRange + 1f && timer > backDownDuration)
        {
            boss.StateMachine.ChangeState(new FGDecisionState(boss));
        }
    }
}
