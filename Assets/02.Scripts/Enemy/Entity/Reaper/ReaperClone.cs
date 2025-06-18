using NiceIO.Sysroot;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ReaperClone : MonsterBase
{
    [Header("마왕(분신) 설정")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject slashNormal;

    private bool isLeft = true;
    private Coroutine currentPattern;

    private BossAnimationHandler bossAnimationHandler;
    private NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();
        bossAnimationHandler = GetComponent<BossAnimationHandler>();
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

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (Health <= 0)
        {
            Health = 0;
            StateMachine.ChangeState(new AirDeadState(this));
        }
        else
        {
            StateMachine.ChangeState(new AirDamageState(this));
        }
    }

    public override void Attack()
    {
        // 공격 도중이라면 리턴
        if (currentPattern != null) return;

        // 기본 공격
        currentPattern = StartCoroutine(NormalAttack(slashNormal));
    }

    public IEnumerator NormalAttack(GameObject effect)
    {
        AnimationHandler.Attack();
        StartCoroutine(ShowAttackEffect(effect));

        Collider2D hit = Physics2D.OverlapCircle(slashNormal.transform.position, AttackRange, playerLayer);

        if (hit != null)
            hit.GetComponent<IDamagable>()?.TakeDamage(AttackPower);

        yield return new WaitForSeconds(5f);

        currentPattern = null;
        StateMachine.ChangeState(new AirIdleState(this));
    }

    IEnumerator ShowAttackEffect(GameObject effect)
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        effect.SetActive(false);
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
}
