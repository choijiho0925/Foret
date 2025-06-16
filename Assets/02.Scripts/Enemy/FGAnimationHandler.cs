using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGAnimationHandler : MonsterAnimationHandler
{
    private static readonly int isRunning = Animator.StringToHash("IsRunning");
    private static readonly int chargeTrigger = Animator.StringToHash("ChargeTrigger");
    private static readonly int teleportAttackTrigger = Animator.StringToHash("TeleportAttackTrigger");

    private SpriteRenderer sprite;

    public SpriteRenderer Sprite
    {
        get { return sprite; }
    }
    
    protected override void Awake()
    {
        base.Awake();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // ForestGuardian 전용 애니메이션 메서드 추가
    public void PlayRun(bool isRun)
    {
        animator.SetBool(isRunning, isRun);
    }

    public void PlayCharge()
    {
        animator.ResetTrigger(chargeTrigger);
        animator.SetTrigger(chargeTrigger);
    }

    public void PlayTeleportAttack()
    {
        animator.ResetTrigger(teleportAttackTrigger);
        animator.SetTrigger(teleportAttackTrigger);
    }

    public void SetFlip(bool flipX)
    {
        if (sprite != null)
            sprite.flipX = flipX;
    }

    public void ResetRotation()
    {
        if (sprite != null)
            sprite.transform.rotation = Quaternion.identity;
    }

    public void SetRotation(float zRotation)
    {
        if (sprite != null)
            sprite.transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }

    public void ResetAllAnimation()
    {
        animator.SetBool(isRunning, false);
        animator.ResetTrigger(chargeTrigger);
        animator.ResetTrigger(teleportAttackTrigger);
    }
}
