using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePlatform : MonoBehaviour
{
    [SerializeField] GameObject[] platforms;
    [SerializeField] GameManager gameManager;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
        }
    }

    private void Update()
    {
        CheckPhase1Clear();
    }

    private void CheckPhase1Clear()
    {
        if (gameManager.isSecondPhase)
        {
            StartCoroutine(ActivatePlatforms());
        }
    }

    private IEnumerator ActivatePlatforms()
    {
        foreach (GameObject platform in platforms)
        {
            if (platform != null)
            {
                platform.SetActive(true);
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
}
