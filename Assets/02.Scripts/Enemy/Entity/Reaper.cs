using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Reaper : BossBase
{
    [Header("마왕(진) 설정")]
    [SerializeField] private int attackCount = 0;           // 패턴 실행을 위한 공격 횟수
    [SerializeField] private float attackDelay = 3f;        // 다음 공격까지 딜레이
    [SerializeField] private float teleportDistance = 3f;   // 순간 이동 시 플레이어와의 거리
    [SerializeField] private GameObject minionPrefab;       // 소환할 몬스터 프리팹
    [SerializeField] private int minionCount = 3;           // 몬스터 갯수
    [SerializeField] private GameObject clonePrefab;        // 소환할 분신 프리팹
    [SerializeField] private int cloneCount = 2;            // 분신 갯수
    [SerializeField] private LayerMask playerLayer;         // 공격을 위한 레이어
    [SerializeField] private GameObject mainSprite;         // 순간 이동 시 사라지게 할 스프라이트
    [SerializeField] private GameObject slashNormal;        // 기본 공격 오브젝트
    [SerializeField] private GameObject slashWide;          // 긴 공격 오브젝트

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
        StateMachine.ChangeState(new ReaperIdleState(this));    // 시작 시 Idle 상태 전환
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

    #region 이동
    public override void Move()
    {
        LookDirection();

        agent.speed = MoveSpeed;
        agent.stoppingDistance = AttackRange * 0.9f;

        Vector2 targetPos = Player.transform.position;

        agent.SetDestination(targetPos);
    }
    #endregion

    #region 기본 공격
    public override void Attack()
    {
        // 공격 도중이라면 리턴
        if (currentPattern != null) return;

        LookDirection();

        // 기본 공격
        currentPattern = StartCoroutine(NormalAttack(slashNormal));
    }

    public IEnumerator NormalAttack(GameObject effect)
    {
        AnimationHandler.Attack();
        StartCoroutine(ShowAttackEffect(effect));

        yield return new WaitForSeconds(0.1f);

        Collider2D hit = Physics2D.OverlapCircle(slashNormal.transform.position, AttackRange, playerLayer);

        if (hit != null)
            hit.GetComponent<IDamagable>()?.TakeDamage(AttackPower);

        yield return new WaitForSeconds(attackDelay);

        currentPattern = null;
        attackCount++;
        StateMachine.ChangeState(new ReaperIdleState(this));
    }

    IEnumerator ShowAttackEffect(GameObject effect)
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        effect.SetActive(false);
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

            Flip(slashNormal.transform);
            Flip(slashWide.transform);
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

    #region 피격
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (IsInvincible) return;
        Health -= damage;

        if (Health <= 0)
        {
            StateMachine.ChangeState(new ReaperDeadState(this));
        }

        bossAnimationHandler.Damage();
        StateMachine.ChangeState(new ReaperDamageState(this));
    }
    #endregion

    #region 패턴
    public IEnumerator SummonClones()
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < cloneCount; i++)
        {
            Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * 2f;
            Instantiate(clonePrefab, pos, Quaternion.identity);
        }

        yield return new WaitForSeconds(1f);
    }

    public IEnumerator SummonMinions()
    {
        yield return new WaitForSeconds(0.2f);
        var sprite = mainSprite.GetComponent<SpriteRenderer>();

        // 무적 처리
        IsInvincible = true;
        sprite.color = new Color(100/255f, 100/255f, 100/255f);

        // 잡몹 소환
        for (int i = 0; i < minionCount; i++)
        {
            Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * 2f;
            GameObject minion = Instantiate(minionPrefab, pos, Quaternion.identity);
        }

        // 일정 시간 대기
        yield return new WaitForSeconds(10f);

        // 무적 해제
        IsInvincible = false;
        sprite.color = new Color(1f, 1f, 1f);
    }

    public IEnumerator TeleportAttack()
    {
        // 플레이어 등쪽으로 순간이동
        StartCoroutine(Teleport());

        yield return new WaitForSeconds(2f);
    }

    public IEnumerator Teleport()
    {
        // 순간이동 애니메이션 실행
        bossAnimationHandler.Teleport();
        yield return new WaitForSeconds(0.4f);

        // 스프라이트 비활성화 및 대기
        mainSprite.SetActive(false);
        yield return new WaitForSeconds(2f);

        // 플레이어 뒤에서 출현
        Appear();
        yield return new WaitForSeconds(0.4f);

        // 긴 공격 실행
        bossAnimationHandler.Attack2();
        StartCoroutine(LongAttack(slashWide));
    }

    private void Appear()
    {
        float playerFacing = Player.transform.localScale.x > 0 ? 1f : -1f;
        Vector2 offset = new Vector2(-playerFacing, 0f) * teleportDistance;
        transform.position = Player.transform.position + (Vector3)offset;

        LookDirection();

        mainSprite.SetActive(true);
        bossAnimationHandler.Appear();
    } 

    private IEnumerator LongAttack(GameObject effect)
    {
        StartCoroutine(ShowAttackEffect(effect));

        Vector2 attackPos = (Vector2)slashWide.transform.position + (Vector2)(transform.up * 0.5f);
        Vector2 size = new Vector2(15f, 2.5f); // 캡슐 범위
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

        yield return new WaitForSeconds(attackDelay);
    }
    #endregion

    private void OnDrawGizmos() // 보스 공격 범위 (추후 삭제)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(slashNormal.transform.position, AttackRange);

        Vector2 attackPos = (Vector2)slashWide.transform.position + (Vector2)(transform.up * 0.75f);
        Vector2 size = new Vector2(15f, 2.5f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(attackPos, size);
    }
}
