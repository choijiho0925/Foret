using UnityEngine;

public class AirDamageState : IState
{
    private MonsterBase monster;

    public AirDamageState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        // Damage 상태 진입

    }

    public void Exit()
    {
        // Damage 상태 해제
    }

    public void Update()
    {

    }
}
