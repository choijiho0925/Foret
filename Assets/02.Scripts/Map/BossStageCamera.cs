using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageCamera : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] CinemachineVirtualCamera bossCamera;
    [SerializeField] CinemachineConfiner2D confiner;
    [SerializeField] CinemachineVirtualCamera reaperCamera;
    [SerializeField] PolygonCollider2D mainArea;
    [SerializeField] PolygonCollider2D bossArea;
    [SerializeField] GameObject backGround;

    private bool isOneTime = true;
    private int currentPriority = 5;
    private int activePriority = 15;

    public CinemachineBrain _brain;

    private void Update()
    {
        if (reaperCamera.transform.position.y >= -80 && GameManager.Instance.isFirstPhaseEnd)
        {
            if (isOneTime)
            {
                MakeCinemachineBrain();
            }
            GameManager.Instance.isFirstPhaseEnd = false;
            reaperCamera.Priority = 5;
            backGround.SetActive(true);
            StartCoroutine(CheckTime());
        }
    }

    private void MakeCinemachineBrain()
    {
        var oldUpdateMethod = _brain.m_UpdateMethod;
        var oldBlendUpdateMethod = _brain.m_BlendUpdateMethod;
        var oldShowDebugText = _brain.m_ShowDebugText;
        var oldIgnoreTimeScale = _brain.m_IgnoreTimeScale;

        DestroyImmediate(_brain);

        var newBrain = _mainCamera.gameObject.AddComponent<CinemachineBrain>();

        var defalutBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut, 2f);
        newBrain.m_DefaultBlend = defalutBlend;

        isOneTime = false;
    }

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

    private IEnumerator CheckTime()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.isSecondPhase = true;
    }
}