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

    public void OnChargeEndAnimationEvent()
    {
        if(fg != null)
        {
            fg.OnChargeAnimationEvent();
        }
    }
}
