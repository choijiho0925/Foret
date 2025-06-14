using UnityEngine;
using UnityEngine.AI;

public class Bat : MonsterBase
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
        agent.speed = MoveSpeed;
        agent.stoppingDistance = AttackRange * 0.9f;
        agent.SetDestination(Player.transform.position);
    }

    public override void Attack()
    {
        timeSinceLastAttack += Time.deltaTime;

        if (timeSinceLastAttack < attackCooltime) return;

        AnimationHandler.Attack(); 
        timeSinceLastAttack = 0f;
    }

    public void ShootProjectile()
    {
        GameObject projectile = Instantiate(monsterProjectilePrefab, projectilePos.position, Quaternion.identity);
        Vector3 dir = (Player.transform.position - projectilePos.position).normalized;
        projectile.GetComponent<MonsterProjectile>().Initialize(dir, AttackPower);
    }
}
