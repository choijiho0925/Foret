using UnityEngine;

public class AirIdleState : IState
{
    private MonsterBase monster;

    public AirIdleState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        // Idle 상태 진입
    }

    public void Exit()
    {
        // Idle 상태 해제
    }

    public void Update()
    {
        // 플레이어 탐지 시 상태 전환
        float distance = Vector3.Distance(monster.transform.position, monster.Player.transform.position);
        if (distance < monster.DetectionRange)
        {
            
        }
    }
}
