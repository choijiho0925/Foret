using UnityEngine;

public class DBDecisionState : IState
{
    private DeathBringer boss;

    public DBDecisionState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        int randomIndex = Random.Range(0, 2);

        switch (randomIndex)
        {
            case 0:
                boss.StateMachine.ChangeState(new DBSweepDropState(boss));
                break;
            case 1:
                boss.StateMachine.ChangeState(new DBDropAttackState(boss));
                break;
        }
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }
}
