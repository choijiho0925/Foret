using System.Collections;
using UnityEngine;

public class FGAnimationEvent : MonoBehaviour
{
    private ForestGuardian fg;

    [SerializeField] private EnemyAttackHitbox normalHitbox;
    [SerializeField] private EnemyAttackHitbox rotatedHitbox;

    private EnemyAttackHitbox currentHitbox;

    private void Awake()
    {
        fg = GetComponentInParent<ForestGuardian>();
    }

    private IEnumerator Start()
    {
        // Warm-up: 히트박스를 한 번 활성화/비활성화하여 Collider 캐싱 유도
        normalHitbox.SetHitboxActive(true);
        rotatedHitbox.SetHitboxActive(true);
        yield return null;
        normalHitbox.SetHitboxActive(false);
        rotatedHitbox.SetHitboxActive(false);
    }

    // 히트 박스 방향 전환
    private void FlipAttackHitboxByFacing()
    {
        if (fg == null) return;

        bool isLookAtLeft = fg.Sprite.flipX;
        float zRotation = fg.Sprite.transform.localEulerAngles.z;

        bool isRotated = Mathf.Approximately(zRotation, 90f) || Mathf.Approximately(zRotation, 270f);
        currentHitbox = isRotated ? rotatedHitbox : normalHitbox;

        // 좌우 방향만 반전 (Collider offset 기준)
        currentHitbox.FlipOffsetX(isLookAtLeft);
    }

    // 히트박스 활성화
    public void EnableAttackHitbox()
    {
        fg.SetAllowLookAtPlayer(false);
        fg.FGSFX.PlayAttackClip();
        FlipAttackHitboxByFacing();

        // 다른 히트박스는 비활성화
        normalHitbox.SetHitboxActive(false);
        rotatedHitbox.SetHitboxActive(false);
        currentHitbox.SetHitboxActive(true);
    }

    // 히트박스 비활성화
    public void DisableAttackHitbox()
    {
        if (currentHitbox != null)
            currentHitbox.SetHitboxActive(false);

        fg.SetAllowLookAtPlayer(true);
    }

    // 차지 애니메이션 시작 이벤트 호출
    public void OnChargeStartEvent()
    {
        if (fg != null)
        {
            fg.OnChargeStart();
            fg.SetAllowLookAtPlayer(false);
        }
    }

    // 차지 + 공격 애니메이션 종료 이벤트 호출
    public void OnChargeAndAttackEndEvent()
    {
        if (fg != null)
        {
            fg.OnChargeAndAttackEnd();
            fg.SetAllowLookAtPlayer(true);
        }
    }

    // 차지 애니메이션 종료 후 공격 애니메이션 호출
    public void OnChargeEndAnimationEvent()
    {
        if (fg != null)
        {
            fg.OnChargeAnimationEvent();
        }
    }

    // 텔레포트 애니메이션 시작 이벤트 호출
    public void OnTeleportAttackStartEvent()
    {
        fg.FGSFX.PlayTeleportClip();

        if (fg != null)
        {
            fg.SetAllowLookAtPlayer(false);
            fg.OnTeleportAttackStart();
        }
    }

    // 텔레포트 애니메이션 종료 이벤트 호출
    public void OnTeleportAttackEndEvent()
    {
        if (fg != null)
        {
            fg.SetAllowLookAtPlayer(true);
            fg.OnTeleportAttackEnd();
        }
    }
}