using UnityEngine;

public class AirChaseState : IState
{
    private MonsterBase monster;

    public AirChaseState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        // Chase 상태 진입
        monster.AnimationHandler.Move(true);
    }

    public void Exit()
    {
        // Chase 상태 해제
        monster.AnimationHandler.Move(false);
    }

    public void Update()
    {
        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(monster.transform.position, monster.Player.transform.position);

        // 플레이어가 감지 범위 밖으로 나갔을 경우
        if (distance > monster.DetectionRange)
        {
            // 대기 상태 전환
            monster.StateMachine.ChangeState(new AirIdleState(monster));
            return;
        }

        // 공격 사거리 안에 플레이어가 들어왔을 경우
        if (distance <= monster.AttackRange - 0.5f)
        {
            // 공격 상태로 전환
            monster.StateMachine.ChangeState(new AirAttackState(monster));
            return;
        }

        // 이동
        monster.Move();
    }
}
