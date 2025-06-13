using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGMeleeState : IState
{
    private ForestGuardian boss;

    public FGMeleeState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.StartCoroutine(MeleeAttack());
        Debug.Log("FGMeleeState 진입");
    }

    public void Exit() { }

    public void Update() { }

    private IEnumerator MeleeAttack()
    {
        // 공격
        boss.Attack();

        yield return new WaitForSeconds(boss.patternDelay);

        boss.StateMachine.ChangeState(new FGDecisionState(boss));

    }
}
