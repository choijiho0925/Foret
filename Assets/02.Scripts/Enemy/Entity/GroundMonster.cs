using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GroundMonster : MonsterBase
{
    [Header("이동")]
    public float patrolDistance = 5f;
    public LayerMask mapLayer;
    public Transform groundCheck;
    public Transform wallCheck;
    public float attackCooldown = 1.5f;

    private Rigidbody2D rb;
    private bool isMovingRight = false;
    private float startX;
    private float attackTimer = 0f;
    private float flipCooldown = 0f;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();

        // 플레이어와의 충돌 무시
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            Collider2D monsterCollider = GetComponent<Collider2D>();
            Collider2D playerCollider = player.GetComponent<Collider2D>();

            if (monsterCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(monsterCollider, playerCollider);
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        startX = transform.position.x;

        if (StateMachine.CurrentState is GroundIdleState)
        {
            StateMachine.ChangeState(new GroundPatrolState(this));
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Health <= 0 && !(StateMachine.CurrentState is GroundDeadState))
        {
            StateMachine.ChangeState(new GroundDeadState(this));
        }
    }

    public override void Move()
    {
        float dir = isMovingRight ? 1f : -1f;
        Debug.Log($"Move called. dir: {dir}, flipCooldown: {flipCooldown}");

        rb.velocity = new Vector2(dir * MoveSpeed, rb.velocity.y);
        Debug.Log($"Velocity set to {rb.velocity}");

        AnimationHandler.Move(true);

        flipCooldown -= Time.deltaTime;
        flipCooldown = Mathf.Max(flipCooldown, 0f);  // 음수 방지

        bool isGroundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, 1f, mapLayer);
        bool isWallAhead = Physics2D.Raycast(wallCheck.position, Vector2.right * dir, 1f, mapLayer);

        Debug.DrawRay(groundCheck.position, Vector2.down * 0.3f, Color.red);
        Debug.DrawRay(wallCheck.position, Vector2.right * dir * 0.3f, Color.green);

        Debug.Log($"Ground: {isGroundAhead}, Wall: {isWallAhead}");

        if (flipCooldown <= 0f &&
            (!isGroundAhead || isWallAhead ||
            (isMovingRight && transform.position.x >= startX + patrolDistance) ||
            (!isMovingRight && transform.position.x <= startX - patrolDistance)))
        {
            Flip();
            flipCooldown = 0.5f;
            Debug.Log("Flip triggered and cooldown reset.");
        }
    }

    public void Flip()
    {
        isMovingRight = !isMovingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        Vector2 v = rb.velocity;
        v.x *= -1;
        rb.velocity = v;
    }

    public void LookDirection()
    {
        bool shouldFaceLeft = Player.transform.position.x < transform.position.x;

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
