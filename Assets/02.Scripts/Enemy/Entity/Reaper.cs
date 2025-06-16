using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Reaper : BossBase
{
    [Header("마왕(진) 설정")]
    [SerializeField] private int attackCount = 0;
    [SerializeField] private float teleportDistance = 3f;
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private int minionCount = 3;
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private int cloneCount = 2;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject slashNormal;
    [SerializeField] private GameObject slashWide;

    private bool isLeft = true;

    private BossAnimationHandler bossAnimationHandler;
    private NavMeshAgent agent;

    public BossAnimationHandler BossAnimationHandler => bossAnimationHandler;

    protected override void Awake()
    {
        base.Awake();
        bossAnimationHandler = GetComponent<BossAnimationHandler>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    protected override void Start()
    {
        StateMachine.ChangeState(new ReaperIdleState(this));
    }
    protected override void Update()
    {
        base.Update();

        // 기본 공격을 patternDelay만큼 한 이후 패턴 실행
        if (attackCount >= patternDelay)
        {
            StateMachine.ChangeState(new ReaperDecisionState(this));
            attackCount = 0;
            return;
        }
    }

    public override void Move()
    {
        LookDirection();

        agent.speed = MoveSpeed;
        agent.stoppingDistance = AttackRange * 0.9f;

        Vector2 targetPos = Player.transform.position;

        agent.SetDestination(targetPos);
    }

    #region 기본 공격
    public override void Attack()
    {
        // 공격 도중이라면 리턴
        if (currentPattern != null) return;

        // 기본 공격
        currentPattern = StartCoroutine(NormalAttack(slashNormal));
    }

    IEnumerator ShowAttackEffect(GameObject effect)
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        effect.SetActive(false);
    }

    public IEnumerator NormalAttack(GameObject effect)
    {
        AnimationHandler.Attack();
        StartCoroutine(ShowAttackEffect(effect));

        Collider2D hit = Physics2D.OverlapCircle(slashNormal.transform.position, AttackRange, playerLayer);

        if (hit != null)
            hit.GetComponent<IDamagable>()?.TakeDamage(AttackPower);

        yield return new WaitForSeconds(3f);

        currentPattern = null;
        attackCount++;
        Debug.Log(attackCount);
        StateMachine.ChangeState(new ReaperIdleState(this));
    }
    #endregion

    private void OnDrawGizmos() // 보스 공격 범위 (추후 삭제)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(slashNormal.transform.position, AttackRange);

        Vector2 attackPos = (Vector2)slashWide.transform.position + (Vector2)(transform.up * 0.5f);
        Vector2 size = new Vector2(12f, 2.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPos, size);
    }

    #region 방향 전환
    public void LookDirection()
    {
        // 플레이어 위치에 맞게 보는 방향 수정
        bool shouldLeft = Player.transform.position.x < transform.position.x;

        if (shouldLeft != isLeft)
        {
            isLeft = shouldLeft;

            SpriteRenderer.flipX = !isLeft;

            FlipSprite(slashNormal.transform);
            FlipSprite(slashWide.transform);
        }
    }

    private void FlipSprite(Transform sprite)
    {
        // 위치 x를 반전시킴
        Vector3 localPos = sprite.localPosition;
        localPos.x *= -1;
        sprite.localPosition = localPos;

        // Sprite가 있다면 flipX도 반전
        var sr = sprite.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = !sr.flipX;
        }
    }
    #endregion

    public override void TakeDamage(int damage)
    {
        if (IsInvincible) return;
        Health -= damage;

        if (Health <= 0)
        {
            StateMachine.ChangeState(new ReaperDeadState(this));
        }

        bossAnimationHandler.Damage();
        StateMachine.ChangeState(new ReaperDamageState(this));
    }

    #region 패턴
    public IEnumerator SummonClones()
    {
        for (int i = 0; i < cloneCount; i++)
        {
            Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * 2f;
            Instantiate(clonePrefab, pos, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator SummonMinions()
    {
        // 무적 처리
        IsInvincible = true;

        // 잡몹 소환
        for (int i = 0; i < minionCount; i++)
        {
            Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * 2f;
            GameObject minion = Instantiate(minionPrefab, pos, Quaternion.identity);
        }

        // 일정 시간 대기
        yield return new WaitForSeconds(2f);

        // 무적 해제
        IsInvincible = false;
    }

    public IEnumerator TeleportAttack()
    {
        Vector2 originPos = transform.position;

        // 플레이어 등쪽으로 순간이동
        float playerFacing = Player.transform.localScale.x > 0 ? 1f : -1f;
        Vector2 offset = new Vector2(-playerFacing, 0f) * teleportDistance;
        transform.position = Player.transform.position + (Vector3)offset;

        LookDirection();

        // 긴 공격 실행
        LongAttack(slashWide);

        yield return new WaitForSeconds(2f);

        transform.position = originPos;
    }

    private void LongAttack(GameObject effect)
    {
        StartCoroutine(ShowAttackEffect(effect));

        Vector2 attackPos = (Vector2)slashWide.transform.position + (Vector2)(transform.up * 0.5f);
        Vector2 size = new Vector2(12f, 2.5f); // 캡슐 범위
        float angle = 0f; // 수평 방향

        Collider2D hit = Physics2D.OverlapCapsule(
            attackPos,
            size,
            CapsuleDirection2D.Horizontal,
            angle,
            playerLayer
        );

        if (hit != null)
            hit.GetComponent<IDamagable>()?.TakeDamage(AttackPower);
    }
    #endregion
}
