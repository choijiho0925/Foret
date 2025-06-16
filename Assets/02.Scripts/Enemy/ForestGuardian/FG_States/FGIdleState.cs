using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGIdleState : IState
{
    private ForestGuardian boss;

    public FGIdleState(ForestGuardian boss)
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
        // 플레이어 거리 등 체크해서 다시 상태 전이 가능
        float distance = boss.GetPlayerDistance();
        if (distance < boss.BackdownRange)
        {
            boss.StateMachine.ChangeState(new FGBackdownState(boss));
        }
    }
}
