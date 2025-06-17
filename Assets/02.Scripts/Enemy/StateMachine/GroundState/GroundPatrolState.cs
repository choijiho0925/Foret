using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPatrolState : IState
{
    private GroundMonster monster;

    public GroundPatrolState(MonsterBase monster)
    {
        this.monster = (GroundMonster)monster;
    }

    public void Enter() 
    {
        monster.AnimationHandler.Move(true);
    }

    public void Exit() 
    {
        monster.AnimationHandler.Move(false);
    }

    public void Update()
    {
        float distance = Vector2.Distance(monster.Player.transform.position, monster.transform.position);

        if (distance < monster.AttackRange)
        {
            monster.StateMachine.ChangeState(new GroundAttackState(monster));
            return;
        }

        monster.Move();
    }
}
