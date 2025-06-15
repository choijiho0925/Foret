using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    None,
    Player,
    Bat,
}
public class ProjectilePool : ObjectPool<ProjectileType, Projectile>
{
    
}
