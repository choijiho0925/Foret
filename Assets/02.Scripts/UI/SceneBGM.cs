using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    public AudioClip forestBGM;
    public AudioClip darkForestBGM;

    private AudioSource audioSource;
    private GameManager gameManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.Instance;
    }

    private void Start()
    {
        if (audioSource != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.RegisterBGMSource(audioSource);
            Debug.Log($"[SceneBGM] BGM등록 완료 : {audioSource.clip?.name}");
        }
        else
        {
            Debug.LogWarning("[SceneBGM] AudioSource 또는 AudioManager를 찾을 수 없습니다.");
        }

        PlayBGM();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlayerReviveEvent>(PlayerReviveHandler);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<PlayerReviveEvent>(PlayerReviveHandler);
    }

    private void PlayBGM()
    {
        if (!gameManager.CanGoNextStage)
        {
            // 1 스테이지
            audioSource.clip = forestBGM;
        }
        else
        {
            // 2 스테이지
            audioSource.clip = darkForestBGM;
        }
        audioSource.Play();
    }

    private void PlayerReviveHandler(PlayerReviveEvent evet)
    {
        PlayBGM();
    }
}
