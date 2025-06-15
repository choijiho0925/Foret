using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    None,
    Player,
    Needle,
    Poison,
    Skull,
}
public class ProjectilePool : ObjectPool<ProjectileType, Projectile>
{
    
}
