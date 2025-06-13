using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGChaseState : IState
{
    private ForestGuardian boss;

    public FGChaseState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter() { Debug.Log("FGChaseState 진입"); }

    public void Exit() { }

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
