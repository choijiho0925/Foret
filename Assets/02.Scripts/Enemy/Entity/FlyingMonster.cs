using UnityEngine;
using UnityEngine.AI;

public class FlyingMonster : MonsterBase
{
    private NavMeshAgent agent;

    [Header("원거리 공격")]
    [SerializeField] private GameObject monsterProjectilePrefab;
    [SerializeField] private Transform projectilePos;
    [SerializeField] private float attackCooltime = 1.5f;

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
        // 플레이어 위치에 맞게 보는 방향 수정
        if (Player.transform.position.x < transform.position.x)
        {
            SpriteRenderer.flipX = false;
        }
        else if (Player.transform.position.x > transform.position.x)
        {
            SpriteRenderer.flipX = true;
        }
    }

    public void ShootProjectile()
    {
        
        GameObject projectile = Instantiate(monsterProjectilePrefab, projectilePos.position, Quaternion.identity);

        Vector3 targetPos = Player.transform.position + Vector3.up;
        Vector3 dir = (targetPos - projectilePos.position).normalized;

        projectile.GetComponent<MonsterProjectile>().Initialize(dir, AttackPower);
    }
}
