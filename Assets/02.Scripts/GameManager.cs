using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Vector3 respawnPoint { get; private set; } //플레이어 리스폰 지점
    public bool CanGoNextStage;
    public bool isSecondPhase;

    public void SetRespawnPoint(Vector3 point)
    {
        respawnPoint = point;
    }
}
