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
        boss.AnimationHandler.Dead();
        boss.Die();
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
}
