using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    None,
    Player,
    Fireball,
    Poison,
    Skull,
    EnergyBall,
}
public class ProjectilePool : ObjectPool<ProjectileType, Projectile>
{
    
}
