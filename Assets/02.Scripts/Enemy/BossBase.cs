using System.Collections;
using UnityEngine;

public class BossBase : MonsterBase
{
    // 패턴 간 딜레이
    public float patternDelay;

    // 플레이어가 범위 안에 들어왔는지
    private bool playerInRange = false;

    protected bool isPatterning = false;    // 현재 패턴 실행 중인지
    protected Coroutine currentPattern;     // 실행 중인 패턴 코루틴 참조

    protected override void Awake()
    {
        base.Awake();

        GameObject player = GameObject.FindWithTag("Player");

        if(player != null)
        {
            Collider2D bossCollider = GetComponent<Collider2D>();
            Collider2D playerCollider = player.GetComponent<Collider2D>();

            if(bossCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(bossCollider, playerCollider);
            }
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Move()
    {
        BossMove();
    }

    public override void Attack()
    {
        
    }

    protected virtual void BossMove()
    {

    }
}
