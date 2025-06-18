using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    None,
    Mushroom,
    Rat,
    NightBorne,
    Bat,
    FlyingForestMonster,
    Necromancer,
    ForestGuardian,
    Reaper,
}
public class MonsterPool : ObjectPool<MonsterType, MonsterBase>
{

}
