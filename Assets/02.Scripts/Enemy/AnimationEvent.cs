using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private FlyingMonster bat;

    private void Start()
    {
        bat = GetComponentInParent<FlyingMonster>();
    }

    public void ShootProjectile()
    {
        if (bat != null)
        {
            bat.ShootProjectile();
        }
    }
}
