using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGNpcState : IState
{
    private ForestGuardian boss;

    public FGNpcState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // BGM 다시 변경
        AudioManager.Instance.RestoreBeforeBGM();

        // 대화 가능 및 돌 생성
        boss.NpcController.CanInteract();
        boss.RunStone.SetActive(true);

        // 방향 고정
        boss.SetAllowLookAtPlayer(false); 

        // Idle 애니메이션으로 복귀
        boss.ResetAllAnimation();

        // 제자리로 복귀
        boss.transform.position = boss.InitialPosition;
    }

    public void Exit() { }
    public void Update() { }
}
