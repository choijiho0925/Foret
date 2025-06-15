using UnityEngine;
using UnityEngine.AI;

public class FlyingMonster : MonsterBase
{
    private NavMeshAgent agent;

    [Header("원거리 공격")]
    [SerializeField] private GameObject monsterProjectilePrefab;
    [SerializeField] private Transform projectilePos;
    [SerializeField] private float attackCooltime = 1.5f;
    [SerializeField] private ProjectileType projectileType;

    private bool isLeft = true;

    private float timeSinceLastAttack = float.MaxValue;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public override void Move()
    {
        LookDirection();

        agent.speed = MoveSpeed;
        agent.stoppingDistance = AttackRange * 0.9f;

        Vector2 targetPos = Player.transform.position;
        targetPos.y += 1f;

        agent.SetDestination(targetPos);
    }

    public override void Attack()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (timeSinceLastAttack < attackCooltime) return;

        LookDirection();
        AnimationHandler.Attack(); 
        timeSinceLastAttack = 0f;
    }

    public void LookDirection()
    {
        // 플레이어 위치에 맞게 보는 방향 및 투사체 발사 위치 수정
        bool shouldLeft = Player.transform.position.x < transform.position.x;

        if (shouldLeft != isLeft)
        {
            isLeft = shouldLeft;

            SpriteRenderer.flipX = !isLeft;

            // projectilePos의 위치 반전
            Vector3 localPos = projectilePos.localPosition;
            localPos.x *= -1f;
            projectilePos.localPosition = localPos;
        }
    }

    public void ShootProjectile()
    {
        //GameObject projectile = Instantiate(monsterProjectilePrefab, projectilePos.position, Quaternion.identity);
        Projectile projectile =
            PoolManager.Instance.ProjectilePool.Get(projectileType, projectilePos.position, Quaternion.identity);

        Vector3 targetPos = Player.transform.position + Vector3.up;
        Vector3 dir = (targetPos - projectilePos.position).normalized;

        //projectile.GetComponent<MonsterProjectile>().Initialize(dir, AttackPower);
        projectile.Initialize(dir, AttackPower);
    }
}
