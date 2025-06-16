using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGNpcState : IState
{
    private ForestGuardian boss;
    private Vector3 npcPosition;
    private bool hasMoved = false;

    public FGNpcState(ForestGuardian boss)
    {
        this.boss = boss;

        // 최초 위치로 이동
        this.npcPosition = boss.InitialPosition;
    }

    public void Enter()
    {
        // Idle 애니메이션으로 복귀
        boss.ResetAllAnimation();

        // npc 레이어로 변경
        boss.gameObject.layer = LayerMask.NameToLayer("Interactable");

        if(!hasMoved)
        {
            boss.transform.position = npcPosition;
            hasMoved = true;
        }
    }

    public void Exit() { }
    public void Update() { }
}
