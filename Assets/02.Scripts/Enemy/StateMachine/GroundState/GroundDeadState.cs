using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDeadState : IState
{
    private MonsterBase monster;
    private bool isCoroutineStarted = false;

    public GroundDeadState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        monster.AnimationHandler.Dead();
        monster.Die();
        if (monster is GroundMonster groundMonster)
        {
            //groundMonster.enabled = false; 
            Rigidbody2D rb = groundMonster.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = Vector2.zero;
        }

        //monster.StartCoroutine(DestroyAfterDelay());
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }

    private IEnumerator DestroyAfterDelay()
    {
        // 애니메이션 재생 시간만큼 대기
        yield return new WaitForSeconds(1.5f);

        // 직접 파괴
        Object.Destroy(monster.gameObject);
    }
}
