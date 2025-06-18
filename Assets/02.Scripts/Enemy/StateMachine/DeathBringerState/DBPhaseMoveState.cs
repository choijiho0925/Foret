using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBPhaseMoveState : IState
{
    private DeathBringer boss;

    public DBPhaseMoveState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        Debug.Log("2페이즈 이동 상태");
        boss.StartCoroutine(PhaseMoveState());
    }

    public void Exit()
    {
        boss.BGOff();
    }

    public void Update()
    {
        
    }

    private IEnumerator PhaseMoveState()
    {
        // 보스 2페이즈 등장
        boss.BossAnimationHandler.PhaseMove();

        // 대기 (대사/연출 추가)
        yield return new WaitForSeconds(5f);

        // 상태 전환
        boss.StateMachine.ChangeState(new DBUpState(boss));        
    }
}
