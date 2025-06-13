using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    private Vector3 respawnPoint;

    private void Start()
    {
        respawnPoint = transform.position; // 초기 위치를 기본 부활 지점으로 설정
    }

    public void SetRespawnPoint(Vector3 newPoint)
    {
        respawnPoint = newPoint; // 새로운 부활 지점 설정
    }

    public void RespawnPlayer()
    {
        transform.position = respawnPoint; // 플레이어를 부활 지점으로 이동
        // 부활할때 체력 -1 
    }
}