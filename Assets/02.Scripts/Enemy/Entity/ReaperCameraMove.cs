using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperCameraMove : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private CinemachineConfiner2D _confiner;
    [SerializeField] private PolygonCollider2D _collider;
    private int currentPriority = 5;
    private int activePriority = 50;

    public void CameraMove()
    {
        if (GameManager.Instance.isFirstPhaseEnd)
        {
            CameraOn();
        }
        else if (_camera.transform.position.y >= -40 && GameManager.Instance.isFirstPhaseEnd)
        {
            CameraOff();
            GameManager.Instance.isFirstPhaseEnd = false;
        }
    }

    private void CameraOn()
    {
        _camera.Priority = activePriority;
        _confiner.m_BoundingShape2D = _collider;
        _confiner.InvalidateCache();
    }

    private void CameraOff()
    {
        _camera.Priority = currentPriority;
    }

}
