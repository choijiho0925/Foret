using UnityEngine;

public class BossAnimationHandler : MonsterAnimationHandler
{
    private static readonly int isAttack2 = Animator.StringToHash("IsAttack2");
    private static readonly int isPattern1 = Animator.StringToHash("IsPattern1");
    private static readonly int isPattern2 = Animator.StringToHash("IsPattern2");
    private static readonly int isTeleport = Animator.StringToHash("IsTeleport");
    private static readonly int isAppear = Animator.StringToHash("IsAppear");

    public void Attack2()
    {
        animator.SetTrigger(isAttack2);
    }

    public void Pattern1()
    {
        animator.SetTrigger(isPattern1);
    }

    public void Pattern2()
    {
        animator.SetTrigger(isPattern2);
    }

    public void Teleport()
    {
        animator.SetTrigger(isTeleport);
    }

    public void Appear()
    {
        animator.SetTrigger(isAppear);
    }
}
