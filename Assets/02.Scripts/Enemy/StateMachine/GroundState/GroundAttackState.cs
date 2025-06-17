using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAttackState : IState
{
    private GroundMonster monster;
    private float attackCooldown = 0f;

    public GroundAttackState(MonsterBase monster)
    {
        this.monster = (GroundMonster)monster;
    }

    public void Enter()
    {
        attackCooldown = 0f;
    }

    public void Exit() { }

    public void Update()
    {
        float distance = Vector2.Distance(monster.Player.transform.position, monster.transform.position);

        // 플레이어가 범위 밖으로 나가면 Patrol로 복귀
        if (distance > monster.AttackRange)
        {
            monster.StateMachine.ChangeState(new GroundPatrolState(monster));
            return;
        }

        monster.LookDirection();

        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            monster.Attack();
            attackCooldown = monster.attackCooldown;
        }
    }
}
