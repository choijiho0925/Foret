using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundIdleState : IState
{
    private MonsterBase monster;

    public GroundIdleState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void Enter() 
    {
        monster.AnimationHandler.Move(false);
    }

    public void Exit() { }

    public void Update()
    {
        float distance = Vector2.Distance(monster.Player.transform.position, monster.transform.position);

        if (distance < monster.DetectionRange)
        {
            monster.StateMachine.ChangeState(new GroundPatrolState(monster));
        }
    }
}
