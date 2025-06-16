using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGAnimationEvent : MonoBehaviour
{
    private ForestGuardian fg;
    [SerializeField] private EnemyAttackHitbox attackHitbox;

    private BoxCollider2D box;

    private Vector2 initialSize;
    private Vector2 initialOffset;

    private bool isRotated90 = false;

    private void Awake()
    {
        fg = GetComponentInParent<ForestGuardian>();
        box = attackHitbox.GetComponent<BoxCollider2D>();
        initialSize = box.size;
        initialOffset = box.offset;
    }

    // 히트 박스 방향 전환
    private void FlipAttackHitboxByFacing()
    {
        if (box == null || fg == null) return;

        // 왼쪽을 보면 true
        bool isLookAtLeft = fg.Sprite.flipX;

        // 텔레포트 등에서 Transform 회전 각도 확인
        float zRotation = fg.Sprite.transform.localEulerAngles.z;

        // 90도 or -90도일 때는 방향 반전 처리
        if (Mathf.Approximately(zRotation, 90f) || Mathf.Approximately(zRotation, 270f))
        {
            // 텔레포트 상태에서 -90도 효과
            if(!isRotated90)
            {
                // size와 offset 교체
                box.size = new Vector2(initialSize.y, initialSize.x);
                box.offset = new Vector2(initialOffset.y, initialOffset.x);

                isRotated90 = true;
            }

            // 좌우 + 상하 반전
            box.offset = new Vector2(
                isLookAtLeft ? -Mathf.Abs(box.offset.x) : Mathf.Abs(box.offset.x),
                -Mathf.Abs(box.offset.y));
        }
        else
        {
            // 텔레포트 아닌 일반 상태로 원복
            if (isRotated90)
            {
                box.size = initialSize;
                box.offset = initialOffset;
                isRotated90 = false;
            }

            // 좌우 반전만 처리
            box.offset = new Vector2(
                isLookAtLeft ? -Mathf.Abs(initialOffset.x) : Mathf.Abs(initialOffset.x),
                initialOffset.y);
        }
    }

    // 히트박스 활성화
    public void EnableAttackHitbox()
    {
        FlipAttackHitboxByFacing();
        attackHitbox.SetHitboxActive(true);
    }

    // 히트박스 비활성화
    public void DisableAttackHitbox()
    {
        attackHitbox.SetHitboxActive(false);
    }

    // 차지 애니메이션 시작 이벤트 호출
    public void OnChargeStartEvent()
    {
        if (fg != null)
        {
            fg.OnChargeStart();
        }
    }

    // 차지 + 공격 애니메이션 종료 이벤트 호출
    public void OnChargeAndAttackEndEvent()
    {
        if (fg != null)
        {
            fg.OnChargeAndAttackEnd();
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
        if(fg != null)
        {
            fg.OnTeleportAttackStart();
        }
    }

    // 텔레포트 애니메이션 종료 이벤트 호출
    public void OnTeleportAttackEndEvent()
    {
        if(fg != null)
        {
            fg.OnTeleportAttackEnd();
        }
    }
}
