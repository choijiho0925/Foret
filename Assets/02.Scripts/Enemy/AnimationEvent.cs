using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private Bat bat;

    private void Start()
    {
        bat = GetComponentInParent<Bat>();
    }

    public void ShootProjectile()
    {
        if (bat != null)
        {
            bat.ShootProjectile();
        }
    }
}
