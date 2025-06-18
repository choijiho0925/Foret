using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private FlyingMonster monster;

    private void Start()
    {
        monster = GetComponentInParent<FlyingMonster>();
    }

    public void ShootProjectile()
    {
        if (monster != null)
        {
            monster.ShootProjectile();
        }
    }
}
