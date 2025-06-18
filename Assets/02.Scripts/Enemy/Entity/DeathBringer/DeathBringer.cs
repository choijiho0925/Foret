using System.Collections;
using System.Linq;
using UnityEngine;

public class DeathBringer : BossBase
{
    [Header("마왕(1페이즈)")]
    [SerializeField] private int attackCount = 0;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private GameObject dropAttackPrefab;
    [SerializeField] private Transform[] dropAttackSpawnPoints;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform attack1Pos;
    [SerializeField] private Transform attack2Pos;
    [SerializeField] private GameObject bg;

    private bool isLeft = true;

    private Rigidbody2D rb;
    private BossAnimationHandler bossAnimationHandler;
    private ReaperCameraMove cm;

    public BossAnimationHandler BossAnimationHandler => bossAnimationHandler;
    public ReaperCameraMove ReaperCameraMove => cm;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        bossAnimationHandler = GetComponent<BossAnimationHandler>();
        cm = GetComponentInChildren<ReaperCameraMove>();
    }

    protected override void Start()
    {
        StateMachine.ChangeState(new DBIdleState(this));
        EventBus.Raise(new BossStartEvent(bossName, bossBGM));
    }

    protected override void Update()
    {
        if (IsDead) return;

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
        IsAttack = true;
        AnimationHandler.Attack();

        yield return new WaitForSeconds(0.3f);

        Vector2 size = new Vector2(6f, 5.5f); // 캡슐 범위
        float angle = 0f; // 수평 방향

        Collider2D hit = Physics2D.OverlapCapsule(
            attack1Pos.position,
            size,
            CapsuleDirection2D.Horizontal,
            angle,
            playerLayer
        );

        if (hit != null)
            hit.GetComponent<IDamagable>()?.TakeDamage(AttackPower);

        yield return new WaitForSeconds(0.7f);

        IsAttack = false;

        yield return new WaitForSeconds(attackDelay);

        currentPattern = null;
        attackCount++;
        StateMachine.ChangeState(new DBIdleState(this));
    }
    #endregion

    #region 피격
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (Health <= 0)
        {
            IsDead = true;
            StateMachine.ChangeState(new DBDeadState(this));
        }
        else
        {
            if (!IsAttack)
                bossAnimationHandler.Damage();

            StateMachine.ChangeState(new DBDamageState(this));
        }
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
        IsAttack = true;

        yield return new WaitForSeconds(0.3f);

        Vector2 size = new Vector2(8f, 5.5f); // 캡슐 범위
        float angle = 0f; // 수평 방향

        Collider2D hit = Physics2D.OverlapCapsule(
            attack2Pos.position,
            size,
            CapsuleDirection2D.Horizontal,
            angle,
            playerLayer
        );

        if (hit != null)
            hit.GetComponent<IDamagable>()?.TakeDamage(AttackPower);

        yield return new WaitForSeconds(0.7f);

        IsAttack = false;

        StartCoroutine(DropAttack());
    }

    public IEnumerator DropAttack()     // 플레이어 위치 위에서 한번 떨어지는 공격
    {
        Vector3 spawnPos = new Vector3(Player.transform.position.x, -122.1f, 0f);
        Instantiate(dropAttackPrefab, spawnPos, Quaternion.identity);

        yield return new WaitForSeconds(attackDelay);
    }

    public IEnumerator DropAttacks()    // 여러 곳에서 떨어지는 공격
    {
        IsAttack = true;

        yield return new WaitForSeconds(0.5f);

        // 위치들을 무작위로 섞고 상위 4개 선택
        Transform[] random = dropAttackSpawnPoints.OrderBy(x => Random.value).ToArray();

        for (int i = 0; i < 4 && i < random.Length; i++)
        {
            Instantiate(dropAttackPrefab, random[i].position, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.5f);

        IsAttack = false;

        yield return new WaitForSeconds(attackDelay);

    }
    #endregion

    // 보스 2페이즈로 이동 후 제거를 위한 함수 (임시)
    public void RemoveGameobject()
    {
        Destroy(gameObject);
    }

    public void BGOff()
    {
        bg.SetActive(false);
    }

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
