using System.Collections;
using UnityEngine;

public class DBDeadState : IState
{
    private DeathBringer boss;

    public DBDeadState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.StartCoroutine(DeadState());
    }

    public void Exit()
    {
        
    }

    public void Update()
    {

    }

    private IEnumerator DeadState()
    {
        // 보스 1페이즈 종료
        boss.BossAnimationHandler.Dead();
        boss.Die();

        Debug.Log("대기 시작");
        // 대기 (대사/연출 추가)
        yield return new WaitForSeconds(5f);
        Debug.Log("대기 끝");

        // 상태 전환
        boss.StateMachine.ChangeState(new DBPhaseMoveState(boss));
    }
}
