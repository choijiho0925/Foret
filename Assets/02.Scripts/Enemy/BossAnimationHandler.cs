using UnityEngine;

public class BossAnimationHandler : MonsterAnimationHandler
{
    private static readonly int isAttack2 = Animator.StringToHash("IsAttack2");
    private static readonly int isSummonC = Animator.StringToHash("IsPattern1");
    private static readonly int isSummonM = Animator.StringToHash("IsPattern2");
    private static readonly int isTeleport = Animator.StringToHash("IsTeleport");
    private static readonly int isAppear = Animator.StringToHash("IsAppear");

    public void LongAttack()
    {
        animator.SetTrigger(isAttack2);
    }

    public void SummonClone()
    {
        animator.SetTrigger(isSummonC);
    }

    public void SummonMinions()
    {
        animator.SetTrigger(isSummonM);
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
