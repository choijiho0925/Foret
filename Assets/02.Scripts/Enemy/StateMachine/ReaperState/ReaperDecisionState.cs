using UnityEngine;

public class ReaperDecisionState : IState
{
    private Reaper boss;

    public ReaperDecisionState(Reaper boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // Decision 상태 진입

        int randomIndex = 0;

        switch (randomIndex)
        {
            case 0:
                boss.StateMachine.ChangeState(new ReaperSummonCloneState(boss));
                break;
            case 1:
                boss.StateMachine.ChangeState(new ReaperSummonMinionsState(boss));
                break;
            case 2:
                boss.StateMachine.ChangeState(new ReaperTeleportState(boss));
                break;
        }
    }

    public void Exit()
    {
        // Decision 상태 해제
    }

    public void Update()
    {
        
    }
}
