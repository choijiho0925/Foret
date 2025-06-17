using UnityEngine;

public class DBAttackState : IState
{
    private DeathBringer boss;

    public DBAttackState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.Attack();
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
}
