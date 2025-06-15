using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera bossCamera;
    [SerializeField] CinemachineConfiner2D confiner;
    [SerializeField] PolygonCollider2D mainArea;
    [SerializeField] PolygonCollider2D bossArea;

    private int currentPriority = 5;
    private int activePriority = 15;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            bossCamera.Priority = activePriority;
            confiner.m_BoundingShape2D = bossArea;
            confiner.InvalidateCache(); // 컨파이너 캐시 무효화
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            bossCamera.Priority = currentPriority;
            confiner.m_BoundingShape2D = mainArea;
            confiner.InvalidateCache(); // 컨파이너 캐시 무효화
        }
    }
}