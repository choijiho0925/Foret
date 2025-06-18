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
        // boss.Health = boss.MaxHealth;
        boss.hasPlayedFirstBGM = false;
        boss.ResetAllAnimation();
    }

    public void Exit()
    {
    }

    public void Update()
    {
        // 플레이어 거리 등 체크해서 다시 상태 전이 가능
        float distance = boss.GetPlayerDistance();

        if (GameManager.Instance.player.isDead)
        {
            return;
        }

        if (distance > boss.DetectionRange)
        {
            boss.StateMachine.ChangeState(new FGReturnState(boss));
            return;
        }

        if (distance < boss.BackdownRange)
        {
            boss.hasPlayedFirstBGM = false;
            boss.StateMachine.ChangeState(new FGBackdownState(boss));
            return;
        }
    }
}
