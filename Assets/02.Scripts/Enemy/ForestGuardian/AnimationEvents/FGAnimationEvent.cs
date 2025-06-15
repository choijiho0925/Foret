using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FGAnimationEvent : MonoBehaviour
{
    private ForestGuardian fg;

    private void Awake()
    {
        fg = GetComponentInParent<ForestGuardian>();
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
        Debug.Log("OnTeleportAttackStartEvent 호출");
        if(fg != null)
        {
            Debug.Log("OnTeleportAttackStart 호출");
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
