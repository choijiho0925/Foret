using System.Collections;
using UnityEngine;

public class ForestGuardian : BossBase
{
    [Header("숲의 주인 설정")]
    [SerializeField] private float backdownRange = 1f;     // 너무 가까우면 회피
    [SerializeField] private float meleeRange = 2f;        // 근접 공격 거리
    [SerializeField] private float chargeRange = 5f;       // 돌진 공격 거리
    [SerializeField] private float teleportRange = 10f;    // 텔레포트 가능 거리
    [SerializeField] private float returnThreshold = 0.1f; // 복귀 거리

    [Header("애니메이션 길이 설정")]
    [SerializeField] private float attackDuration = 0.7f;

    public float AttackDuration => attackDuration;

    // 초기 위치
    public Vector3 InitialPosition { get; private set; }

    // 읽기 전용
    public float BackdownRange => backdownRange;
    public float ChargeRange => chargeRange;
    public float TeleportRange => teleportRange;
    public float ReturnThreshold => returnThreshold;

    // 물리 기반 이동
    private Rigidbody2D rb;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        base.Awake();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        // 초기 위치 저장
        InitialPosition = transform.position;

        Debug.Log("보스 시작됨, 초기 상태: FGReturnState 진입");

        // 초기 상태는 복귀
        StateMachine.ChangeState(new FGReturnState(this));
    }

    private void Update()
    {
        // FSM 업데이트
        StateMachine.Update();

        // 시야 조정
        LookAtPlayer();
    }

    // 플레이어를 바라보게 함
    public void LookAtPlayer()
    {
        if (Player == null) return;

        float dirX = Player.transform.position.x - transform.position.x;

        // 일정 거리 이상일 때만 실행
        if(Mathf.Abs(dirX) > 0.01f)
        {
            // 오른쪽에 있으면 flipX = true
            spriteRenderer.flipX = dirX < 0;
        }
    }

    // 플레이어와의 거리 정보
    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, Player.transform.position);
    }

    // 플레이어에게 이동
    public void MoveToPlayer(Vector3 target)
    {
        Vector2 direction = (target - transform.position).normalized;
        Vector2 moveDelta = direction * MoveSpeed * Time.deltaTime;

        rb.MovePosition(rb.position + moveDelta);
    }

    //// 초기 위치로 돌아가는 이동
    //public void ReturnToInitialPosition()
    //{
    //    Vector2 direction = (InitialPosition - transform.position).normalized;
    //    transform.position += (Vector3)(direction * MoveSpeed * Time.deltaTime);
    //}

    // 회피 거리 확인
    public void BackdownFromPlayer(Vector3 from, float backdownDistance = 2f)
    {
        // 보스 기준으로 플레이어보다 왼쪽이면 오른쪽으로 회피, 오른쪽이면 왼쪽으로 회피
        float direction = Mathf.Sign(transform.position.x - from.x);

        Vector2 moveDelta = Vector2.right * direction * MoveSpeed * 5 * Time.deltaTime;

        rb.MovePosition(rb.position + moveDelta);
    }

    // 플레이어 위로 텔레포트할 수 있는지 확인
    public bool CanTeleportTo(Vector2 targetPos)
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            Debug.LogWarning("보스에 Collider2D가 없음");
            return false;
        }

        Vector2 bossSize = collider.bounds.size;
        LayerMask mapMask = LayerMask.GetMask("Ground", "Top", "Wall");

        Collider2D hit = Physics2D.OverlapBox(targetPos, bossSize, 0f, mapMask);

        Debug.Log($"OverlapBox at {targetPos}, size {bossSize}, hit: {hit?.gameObject.name ?? "None"}");

        return hit == null;
    }

    // 플레이어 위로 텔레포트
    public void TeleportTo(Vector2 position)
    {
        transform.position = position;
    }


    // 달리기 애니메이션
    public void PlayRunAnimation()
    {
        animator.SetBool("IsRunning", true);
    }

    // 공격 애니메이션
    public void PlayeAttackAnimation()
    {
        animator.ResetTrigger("AttackTrigger"); // 중복 방지용
        animator.SetTrigger("AttackTrigger");
    }

    // 피격 애니메이션
    public void PlayHurtAnimation()
    {
        animator.SetTrigger("HurtTrigger");
    }

    // 애니메이션 파라미터 리셋
    public void ResetAllAnimation()
    {
        animator.SetBool("IsRunning", false);
    }

}
