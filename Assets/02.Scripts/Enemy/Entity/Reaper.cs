using System.Collections;
using UnityEngine;

public class Reaper : BossBase
{
    [Header("마왕(진) 설정")]
    [SerializeField] private float teleportDistance = 3f;

    protected override void Start()
    {
        base.Start();

    }

    protected override void BossMove()
    {
        base.BossMove();

    }
}
