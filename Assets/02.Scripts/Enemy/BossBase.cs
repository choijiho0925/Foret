using System;
using System.Collections;
using UnityEngine;

public class BossBase : MonsterBase
{
    // 패턴 간 딜레이
    public float patternDelay;
    public string bossName;
    public AudioClip bossBGM;

    // 플레이어가 범위 안에 들어왔는지
    private bool playerInRange = false;
    private UIManager uiManager;

    private bool isAttack = false;
    protected bool isPatterning = false;    // 현재 패턴 실행 중인지
    protected Coroutine currentPattern;     // 실행 중인 패턴 코루틴 참조

    protected Collider2D bossCollider;
    protected Collider2D playerCollider;

    protected Rigidbody2D _rigidbody;

    public bool IsAttack
    {
        get => isAttack;
        set
        {
            if (isAttack == value) return; // 값이 바뀌었을 때만 설정
            isAttack = value;
        }
    }


    protected override void Awake()
    {
        base.Awake();

        GameObject player = GameObject.FindWithTag("Player");
        uiManager = UIManager.Instance;
        _rigidbody = GetComponent<Rigidbody2D>();

        if(player != null)
        {
             bossCollider = GetComponent<Collider2D>();
             playerCollider = player.GetComponent<Collider2D>();

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

    }

    public override void Attack()
    {

    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        
        //uiManager.UpdateBossHealthBar((float)Health / MaxHealth);
    }


    public override void Die()
    {
        Physics2D.IgnoreCollision(bossCollider, playerCollider, false);
        // npc 레이어로 변경
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        bossCollider.isTrigger = true;

        if (_rigidbody != null)
        {
            _rigidbody.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
