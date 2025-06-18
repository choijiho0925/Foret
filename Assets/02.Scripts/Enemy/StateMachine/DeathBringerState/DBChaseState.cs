using UnityEngine;

public class DBChaseState : IState
{
    private DeathBringer boss;

    public DBChaseState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.AnimationHandler.Move(true);
    }

    public void Exit()
    {
        boss.AnimationHandler.Move(false);
    }

    public void Update()
    {
        if (boss.IsDead)
        {
            boss.StateMachine.ChangeState(new DBDeadState(boss));
            return;
        }

        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(boss.transform.position, boss.Player.transform.position);

        // 플레이어가 감지 범위 밖으로 나갔을 경우
        if (distance > boss.DetectionRange)
        {
            // 대기 상태 전환
            boss.StateMachine.ChangeState(new DBIdleState(boss));
            return;
        }

        // 공격 사거리 안에 플레이어가 들어왔을 경우
        if (distance <= boss.AttackRange)
        {
            // 공격 상태로 전환
            boss.StateMachine.ChangeState(new DBAttackState(boss));
            return;
        }

        // 이동
        boss.Move();
    }
}
