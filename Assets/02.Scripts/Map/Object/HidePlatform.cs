using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePlatform : MonoBehaviour
{
    [SerializeField] GameObject[] platforms;

    //private void Update()
    //{
    //    CheckPhase1Clear();
    //}

    //private void CheckPhase1Clear()
    //{
    //    if (게임매니저의 isPhase2라는 불값)
    //    {
    //        StartCoroutine(ActivatePlatforms());
    //    }
    //}

    //private IEnumerator ActivatePlatforms()
    //{
    //    foreach (GameObject platform in platforms)
    //    {
    //        if (platform != null)
    //        {
    //            platform.SetActive(true);
    //        }
    //        yield return new WaitForSeconds(1.5f);
    //    }
    //}
}
