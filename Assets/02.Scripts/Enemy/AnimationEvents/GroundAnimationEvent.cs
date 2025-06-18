using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAnimationEvent : MonoBehaviour
{
    private GroundMonster monster;

    [Header("애니메이션 이벤트용 히트박스")]
    [SerializeField] private EnemyAttackHitbox attackHitbox;

    private void Awake()
    {
        monster = GetComponentInParent<GroundMonster>();

        if (monster == null)
            Debug.LogError("[GroundAnimationEvent] GroundMonster를 찾을 수 없습니다.");

        if (attackHitbox == null)
            Debug.LogWarning("[GroundAnimationEvent] attackHitbox가 인스펙터에서 설정되지 않았습니다.");
    }

    public void EnableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetHitboxActive(true);
        }
    }

    public void DisableHitbox()
    {
        if (attackHitbox != null)
        {
            attackHitbox.SetHitboxActive(false);
        }
    }

    public void StartAttackMotion()
    {
        if (monster != null)
            monster.StartAttack();
    }

    public void EndAttackMotion()
    {
        if (monster != null)
            monster.EndAttack();
    }
}
