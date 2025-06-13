using UnityEngine;

public class AirAttackState : IState
{
    private MonsterBase monster;

    public AirAttackState(MonsterBase monster)
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
        
    }
}
