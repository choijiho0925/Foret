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
        // Attack 상태 진입
    }

    public void Exit()
    {
        // Attack 상태 해제
    }

    public void Update()
    {
        if (Vector3.Distance(monster.Player.transform.position, monster.transform.position) > monster.AttackRange)
        {
            monster.StateMachine.ChangeState(new AirIdleState(monster));
            return;
        }

        monster.Attack();
    }
}
