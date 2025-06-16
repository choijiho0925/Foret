using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperDecisionState : IState
{
    private BossBase boss;

    public ReaperDecisionState(BossBase boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // Decision 상태 진입
        
    }

    public void Exit()
    {
        // Decision 상태 해제
    }

    public void Update()
    {
        
    }
}
