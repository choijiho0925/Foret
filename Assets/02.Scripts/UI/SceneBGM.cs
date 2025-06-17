using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        if (source != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.RegisterBGMSource(source);
            Debug.Log($"[SceneBGM] BGM등록 완료 : {source.clip?.name}");
        }
        else
        {
            Debug.LogWarning("[SceneBGM] AudioSource 또는 AudioManager를 찾을 수 없습니다.");
        }
    }
}
