using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    public ProjectilePool ProjectilePool { get; private set;}
    

    private void Awake()
    {
        ProjectilePool = GetComponentInChildren<ProjectilePool>();
    }
}
