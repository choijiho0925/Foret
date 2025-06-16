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
    [SerializeField] private float immuneDuration = 1f;    // 무적 시간

    [Header("애니메이션 길이 설정")]
    [SerializeField] private float attackDuration = 0.8f;

    [Header("돌진 공격 충전 시간 설정")]
    [SerializeField] private float chargeDelay = 2f;          // 충전 시간
    [SerializeField] private float chargeDuration = 2f;       // 돌진 지속 시간
    [SerializeField] private float chargeSpeedMultiplier = 50f;  // 돌진 속도 배수

    [Header("회피 설정")]
    private bool isBackdown = false;        // 회피 중인지 체크
    private float backdownCooldown = 0.5f;  // 회피 재진입 대기 시간
    private float backdownTimer = 0f;

    // 사망(패배) 확인 변수
    private bool dead = false;

    // 상태 전환 잠금 변수
    private bool isStateLocked = false;

    private float immuneTimer = 0f;    // 무적 시간 카운터
    private bool isImmune = false;      // 무적 상태 여부

    // 차지 중인지 확인
    private bool isChargeAttacking = false;

    // 텔레포트 공격 중인지 확인
    private bool isTeleportAttacking = false;

    // 초기 위치
    public Vector3 InitialPosition { get; private set; }

    // 플레이어를 바라보고 있는지(회피 시에는 반대)
    private bool allowLookAtPlayer = true;

    // 읽기 전용
    public float BackdownRange => backdownRange;
    public float ChargeRange => chargeRange;
    public float TeleportRange => teleportRange;
    public float ReturnThreshold => returnThreshold;

    public float AttackDuration => attackDuration;

    public float ChargeDelay => chargeDelay;
    public float ChargeDuration => chargeDuration;
    public float ChargeSpeedMultiplier => chargeSpeedMultiplier;

    public SpriteRenderer Sprite => fgAnimationHandler.Sprite;

    // 물리 기반 이동
    private Rigidbody2D rb;

    private Animator animator;

    private FGAnimationHandler fgAnimationHandler;

    private void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        fgAnimationHandler = GetComponent<FGAnimationHandler>();
    }

    protected override void Start()
    {
        base.Start();

        // 초기 위치 저장
        InitialPosition = transform.position;

        // 초기 상태는 복귀
        StateMachine.ChangeState(new FGReturnState(this));
    }

    private void Update()
    {
        // FSM 업데이트
        StateMachine.Update();

        // 시야 조정
        LookAtPlayer();

        // 무적 시간 카운트
        if(isImmune)
        {
            immuneTimer -= Time.deltaTime;

            if(immuneTimer <= 0f)
            {
                isImmune = false;
            }
        }
    }

    // 상태 잠금 설정
    public void LockState()
    {
        isStateLocked = true;
    }

    // 상태 잠금 해제
    public void UnlockState()
    {
        isStateLocked = false;
    }

    // TakeDamage 오버라이드
    public override void TakeDamage(int damage)
    {
        // 죽었으면 데미지 그만
        if (dead) return;

        Health -= damage;
        fgAnimationHandler.Damage();

        // 무적 시작
        isImmune = true;
        immuneTimer = immuneDuration;

        // 상태 잠금 해제
        UnlockState();

        if (Health <= 0 && !dead)
        {
            dead = true;
            PlayDeathAnimation(); // 사망 애니메이션 재생만
            rb.velocity = Vector2.zero; // 움직임 멈춤
        }
    }

    // 플레이어를 바라보게 함
    public void LookAtPlayer()
    {
        if (!allowLookAtPlayer || Player == null) return;

        float dirX = Player.transform.position.x - transform.position.x;

        // 일정 거리 이상일 때만 실행
        if(Mathf.Abs(dirX) > 0.01f)
        {
            // 오른쪽에 있으면 flipX = true
            fgAnimationHandler.SetFlip(dirX < 0);
        }
    }

    // 플레이어를 바라보는 허용 여부
    public void SetAllowLookAtPlayer(bool value)
    {
        allowLookAtPlayer = value;
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

    // 회피
    public bool TryBackdownMove(float direction, float moveDistance)
    {
        Vector2 moveVector = Vector2.right * direction * moveDistance;
        Vector2 targetPos = rb.position + moveVector;

        LayerMask mapMask = LayerMask.GetMask("Ground", "Wall", "Top");
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right * direction, moveDistance + 0.1f, mapMask);

        if (hit.collider == null)
        {
            rb.MovePosition(targetPos);
            fgAnimationHandler.SetFlip(direction < 0);
            return true;
        }

        return false; // 막혔다
    }

    // 벽으로 막혀 있지 않은지 (회피 가능한지) 체크
    public bool CanBackdown(float direction, float checkDistance)
    {
        Vector2 checkDir = Vector2.right * direction;
        LayerMask mask = LayerMask.GetMask("Ground", "Wall", "Top");
        RaycastHit2D hit = Physics2D.Raycast(rb.position, checkDir, checkDistance, mask);
        return hit.collider == null;
    }

    // 플레이어 위로 텔레포트할 수 있는지 확인
    public bool CanTeleportTo(Vector2 targetPos)
    {
        Collider2D collider = GetComponent<Collider2D>();
        if (collider == null)
        {
            return false;
        }

        Vector2 bossSize = collider.bounds.size;
        LayerMask mapMask = LayerMask.GetMask("Ground", "Top", "Wall");

        Collider2D hit = Physics2D.OverlapBox(targetPos, bossSize, 0f, mapMask);

        return hit == null;
    }

    // 플레이어 위로 텔레포트
    public void TeleportTo(Vector2 position)
    {
        transform.position = position;
        StartFallAfterTeleport();
    }

    // 텔레포트 이후 아래로 떨어짐
    public void StartFallAfterTeleport(float fallSpeed = -20f)
    {
        rb.velocity = new Vector2(0, fallSpeed);
    }


    // 달리기 애니메이션
    public void PlayRunAnimation()
    {
        fgAnimationHandler.PlayRun(true);
    }

    // 차지 + 공격 애니메이션에서 호출됨
    public void OnChargeAnimationEvent()
    {
        if(StateMachine.CurrentState is FGChargeAttackState chargeState)
        {
            chargeState.StartCharge();
        }
    }

    // 상태 전이 시도
    public bool TryChangeState(IState newState)
    {
        if (isStateLocked)
        {
            return false;
        }
        StateMachine.ChangeState(newState);
        return true;
    }

    // 차지 애니메이션 시작 시 호출
    public void OnChargeStart()
    {
        if (isChargeAttacking) return;

        isChargeAttacking = true;
        LockState();  // 상태 잠금: 차지 시작
    }

    // 차지 및 공격 애니메이션 종료 시 호출
    public void OnChargeAndAttackEnd()
    {
        if (StateMachine.CurrentState is FGChargeAttackState && isChargeAttacking)
        {
            isChargeAttacking = false;
            UnlockState();

            StartCoroutine(DelayedStateChange());
        }
    }

    // 텔레포트 애니메이션 시작 시 호출
    public void OnTeleportAttackStart()
    {
        if (StateMachine.CurrentState is FGTeleportState teleportState)
        {
            if (isTeleportAttacking) return;

            isTeleportAttacking = true;
            LockState();

            // 시야 고정
            SetAllowLookAtPlayer(false);

            teleportState.StartTeleport(); 
        }
    }

    // 낙하 직전 회전
    public void ApplyTeleportRotation()
    {
        // 회전 방향 결정 (오른쪽을 보고 있으면 true)
        float rotationZ = fgAnimationHandler.Sprite.flipX ? 90f : -90f;
        fgAnimationHandler.SetRotation(rotationZ);
    }

    // 텔레포트 준비 애니메이션 재생
    public void PlayTeleportReadyAnimation()
    {
        fgAnimationHandler.PlayTeleportAttack();
    }

    // 텔레포트 공격 종료 시 호출
    public void OnTeleportAttackEnd()
    {
        if (StateMachine.CurrentState is FGTeleportState && isTeleportAttacking)
        {
            fgAnimationHandler.ResetRotation();
            isTeleportAttacking = false;
            UnlockState();

            SetAllowLookAtPlayer(true);

            StartCoroutine(DelayedStateChange());
        }
    }

    // 패턴 간 딜레이
    private IEnumerator DelayedStateChange()
    {
        yield return new WaitForSeconds(patternDelay);
        this.TryChangeState(new FGDecisionState(this));
    }

    // 충전 애니메이션
    public void PlayChargeAnimation()
    {
        fgAnimationHandler.PlayCharge();
    }

    // 공격 애니메이션
    public void PlayAttackAnimation()
    {
        fgAnimationHandler.Attack();
    }

    // 사망 애니메이션
    public void PlayDeathAnimation()
    {
        fgAnimationHandler.Dead();
    }

    // 애니메이션 파라미터 리셋
    public void ResetAllAnimation()
    {
        fgAnimationHandler.ResetAllAnimation();
    }

}
