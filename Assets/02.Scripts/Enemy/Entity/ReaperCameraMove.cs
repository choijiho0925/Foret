using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperCameraMove : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CinemachineBrain _brain;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private CinemachineConfiner2D _confiner;
    [SerializeField] private PolygonCollider2D _collider;
    [SerializeField] private BossStageCamera _bossStageCamera;

    private bool isOneTime = true;
    private int activePriority = 50;

    private void Update()
    {
        if (isOneTime && GameManager.Instance.isFirstPhaseEnd)
        {
            CameraMove();
            isOneTime = false;
        }
    }

    public void CameraMove()
    {
        if (GameManager.Instance.isFirstPhaseEnd)
        {
            CameraOn();
        }
    }

    private void CameraOn()
    {
        _camera.Priority = activePriority;
        _confiner.m_BoundingShape2D = _collider;
        _confiner.InvalidateCache();

        var oldUpdateMethod = _brain.m_UpdateMethod;
        var oldBlendUpdateMethod = _brain.m_BlendUpdateMethod;
        var oldShowDebugText = _brain.m_ShowDebugText;
        var oldIgnoreTimeScale = _brain.m_IgnoreTimeScale;

        DestroyImmediate(_brain);

        var newBrain = _mainCamera.gameObject.AddComponent<CinemachineBrain>();

        newBrain.m_UpdateMethod = oldUpdateMethod;
        newBrain.m_BlendUpdateMethod = oldBlendUpdateMethod;
        newBrain.m_ShowDebugText = oldShowDebugText;
        newBrain.m_IgnoreTimeScale = oldIgnoreTimeScale;

        var defalutBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);
        newBrain.m_DefaultBlend = defalutBlend;

        _brain = _mainCamera.GetComponent<CinemachineBrain>();
        _bossStageCamera._brain = newBrain;
    }
}
