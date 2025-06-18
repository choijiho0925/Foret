using UnityEngine;

public class MonsterAnimationHandler : MonoBehaviour
{
    private static readonly int isMove = Animator.StringToHash("IsMove");
    private static readonly int isAttack = Animator.StringToHash("IsAttack");
    private static readonly int isDamage = Animator.StringToHash("IsDamage");
    private static readonly int isDead = Animator.StringToHash("IsDead");

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public virtual void Move(bool isMoving)
    {
        animator.SetBool(isMove, isMoving);
    }

    public virtual void Attack()
    {
        animator.SetTrigger(isAttack);
    }

    public virtual void Damage()
    {
        animator.SetTrigger(isDamage);
    }

    public virtual void Dead()
    {
        animator.SetTrigger(isDead);
    }
}
