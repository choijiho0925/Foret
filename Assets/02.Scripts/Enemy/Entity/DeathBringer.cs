using System.Collections;
using UnityEngine;

public class DeathBringer : BossBase
{
    [Header("마왕(1페이즈)")]
    [SerializeField] private int attackCount = 0;
    [SerializeField] private GameObject dropAttackPrefab;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attack1Pos;
    [SerializeField] private Transform attack2Pos;

    private bool isLeft = true;

    private Rigidbody2D rb;
    private BossAnimationHandler bossAnimationHandler;

    public BossAnimationHandler BossAnimationHandler => bossAnimationHandler;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        bossAnimationHandler = GetComponent<BossAnimationHandler>();
    }

    protected override void Start()
    {
        StateMachine.ChangeState(new DBIdleState(this));
    }

    protected override void Update()
    {
        base.Update();

        // 기본 공격을 patternDelay만큼 한 이후 패턴 실행
        if (attackCount >= patternDelay)
        {
            StateMachine.ChangeState(new DBDecisionState(this));
            attackCount = 0;
            return;
        }
    }

    #region 이동
    public override void Move()
    {
        LookDirection();

        MoveToPosition(Player.transform);
    }

    public void MoveToPosition(Transform trans)
    {
        Vector2 direction = ((Vector2)trans.position - rb.position).normalized;
        Vector2 moveDelta = direction * MoveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + moveDelta);
    }
    #endregion

    #region 기본 공격
    public override void Attack()
    {
        // 공격 도중이라면 리턴
        if (currentPattern != null) return;

        LookDirection();

        // 기본 공격
        currentPattern = StartCoroutine(NormalAttack());
    }

    public IEnumerator NormalAttack()
    {
        AnimationHandler.Attack();

        //Vector2 size = new Vector2(6f, 5.5f); // 캡슐 범위
        //float angle = 0f; // 수평 방향

        //Collider2D hit = Physics2D.OverlapCapsule(
        //    attack1Pos.position,
        //    size,
        //    CapsuleDirection2D.Horizontal,
        //    angle,
        //    playerLayer
        //);

        yield return new WaitForSeconds(3f);

        currentPattern = null;
        attackCount++;
        StateMachine.ChangeState(new DBIdleState(this));
    }
    #endregion

    #region 피격
    public override void TakeDamage(int damage)
    {
        if (IsInvincible) return;
        Health -= damage;

        if (Health <= 0)
        {
            StateMachine.ChangeState(new DBDeadState(this));
        }

        bossAnimationHandler.Damage();
        StateMachine.ChangeState(new DBDamageState(this));
    }
    #endregion

    #region 방향 전환
    public void LookDirection()
    {
        // 플레이어 위치에 맞게 보는 방향 수정
        bool shouldLeft = Player.transform.position.x < transform.position.x;

        if (shouldLeft != isLeft)
        {
            isLeft = shouldLeft;

            SpriteRenderer.flipX = !isLeft;

            Flip(attack1Pos);
            Flip(attack2Pos);
        }
    }

    private void Flip(Transform pos)
    {
        // 위치 x를 반전시킴
        Vector3 localPos = pos.localPosition;
        localPos.x *= -1;
        pos.localPosition = localPos;

        // Sprite가 있다면 flipX도 반전
        var sprite = pos.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.flipX = !sprite.flipX;
        }
    }
    #endregion

    #region 패턴
    public IEnumerator SweepAndDrop()
    {
        yield return new WaitForSeconds(0.2f);

        //Vector2 size = new Vector2(8f, 5.5f); // 캡슐 범위
        //float angle = 0f; // 수평 방향

        //Collider2D hit = Physics2D.OverlapCapsule(
        //    attack2Pos.position,
        //    size,
        //    CapsuleDirection2D.Horizontal,
        //    angle,
        //    playerLayer
        //);
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(DropAttack());
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator DropAttack()
    {
        Vector3 spawnPos = Player.transform.position + new Vector3(0f, 10.9f, 0f);
        Instantiate(dropAttackPrefab, spawnPos, Quaternion.identity);

        yield return new WaitForSeconds(1f);
    }
    #endregion

    private void OnDrawGizmos() // 보스 공격 범위 (추후 삭제)
    {
        Vector2 size1 = new Vector2(6f, 5.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attack1Pos.position, size1);

        Vector2 size2 = new Vector2(8f, 5.5f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(attack2Pos.position, size2);
    }
}
