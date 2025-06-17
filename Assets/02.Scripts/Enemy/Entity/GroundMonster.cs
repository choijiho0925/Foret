using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GroundMonster : MonsterBase
{
    [Header("이동")]
    public float patrolDistance = 5f;             // 좌우 이동 범위
    public LayerMask mapLayer;                    // 지형 레이어
    public Transform groundCheck;                 // 바닥 확인용
    public Transform wallCheck;                   // 벽 확인용
    public float attackCooldown = 1.5f;           // 공격 쿨타임

    [Header("기본 방향")]
    [SerializeField]
    private bool isMovingRight = false; // 기본값은 왼쪽 이동

    private Rigidbody2D rb;
    private float startX;                         // 시작 지점 X좌표
    private float attackTimer = 0f;
    private float flipCooldown = 0f;              // 방향 전환 쿨타임

    // 공격 중인지 확인
    private bool isAttacking;
    public bool IsAttacking => isAttacking;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();

        // 플레이어와 충돌 무시
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Collider2D monsterCol = GetComponent<Collider2D>();
            Collider2D playerCol = player.GetComponent<Collider2D>();

            if (monsterCol != null && playerCol != null)
            {
                Physics2D.IgnoreCollision(monsterCol, playerCol);
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        startX = transform.position.x;

        // 기본 상태를 Patrol로 전환
        if (StateMachine.CurrentState is GroundIdleState)
        {
            StateMachine.ChangeState(new GroundPatrolState(this));
        }
    }

    protected override void Update()
    {
        base.Update();

        // 체력이 0 이하면 죽은 상태로 전환
        if (Health <= 0 && !(StateMachine.CurrentState is GroundDeadState))
        {
            StateMachine.ChangeState(new GroundDeadState(this));
        }
    }

    public override void Move()
    {
        if(isAttacking)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        float dir = isMovingRight ? 1f : -1f;
        Vector2 moveDir = new Vector2(dir, 0f);

        // 지면 확인용 레이캐스트 (경사면)
        RaycastHit2D groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.6f, mapLayer);

        if (groundHit)
        {
            Vector2 normal = groundHit.normal;
            float slopeAngle = Vector2.Angle(normal, Vector2.up);

            // 경사면 위일 경우 이동 방향을 기울기에 맞게 보정
            if (slopeAngle > 0f && slopeAngle <= 45f)
            {
                Vector2 slopeDir = new Vector2(normal.y, -normal.x); // 경사면 따라 이동
                moveDir = slopeDir * dir;
            }
        }

        rb.velocity = new Vector2(moveDir.x * MoveSpeed, rb.velocity.y);

        // 중복호출 방지
        bool isActuallyMoving = Mathf.Abs(rb.velocity.x) > 0.05f;
        AnimationHandler.Move(isActuallyMoving); 

        // 방향 전환 조건 확인
        flipCooldown -= Time.deltaTime;
        flipCooldown = Mathf.Max(flipCooldown, 0f);

        bool isGroundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.6f, mapLayer);
        bool isWallAhead = Physics2D.Raycast(wallCheck.position, Vector2.right * dir, 0.6f, mapLayer);

        bool outOfRange =
            (isMovingRight && transform.position.x >= startX + patrolDistance) ||
            (!isMovingRight && transform.position.x <= startX - patrolDistance);

        if (flipCooldown <= 0f && (!isGroundAhead || isWallAhead || outOfRange))
        {
            Flip();
            flipCooldown = 0.5f;
        }

        Debug.DrawRay(groundCheck.position, Vector2.down * 0.6f, Color.red);
        Debug.DrawRay(wallCheck.position, Vector2.right * dir * 0.6f, Color.green);
    }

    public void Flip()
    {
        isMovingRight = !isMovingRight;

        // 방향 반전: 로컬 스케일만 반전 (자식 오브젝트 포함 전체 반영)
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // 공격 시작
    public void StartAttack()
    {
        isAttacking = true;
        AnimationHandler.Move(false);
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    // 공격 종료
    public void EndAttack()
    {
        isAttacking = false;
        AnimationHandler.Move(true);
    }

    public void LookDirection()
    {
        bool shouldFaceLeft = Player.transform.position.x < transform.position.x;

        // 플레이어가 왼쪽에 있는데 현재 오른쪽 보는 중이면 Flip
        if (shouldFaceLeft != !isMovingRight)
        {
            Flip();
        }
    }

    public override void Attack()
    {
        AnimationHandler.Attack();
    }
}