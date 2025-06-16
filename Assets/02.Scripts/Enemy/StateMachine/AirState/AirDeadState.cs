using UnityEngine;

public class AirDeadState : IState
{
    private MonsterBase monster;

    public AirDeadState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        // Dead 상태 진입
        monster.AnimationHandler.Dead();
        monster.Die();
    }

    public void Exit()
    {
        
    }

    public void Update()
    {
        
    }
}
