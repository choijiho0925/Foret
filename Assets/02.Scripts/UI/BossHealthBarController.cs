using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarController : MonoBehaviour
{
    public GameObject healthBarUI;
    public Slider healthBar;
    public TextMeshProUGUI bossNameText;

    private void Start()
    {
        healthBar.value = 1.0f;
        UIManager.Instance.RegisterBossHealthBar(this);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<BossStartEvent>(BossStartHandler);
        EventBus.Subscribe<BossClearEvent>(BossClearHandler);
        EventBus.Subscribe<GameOverEvent>(GameOverHandler);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<BossStartEvent>(BossStartHandler);
        EventBus.UnSubscribe<BossClearEvent>(BossClearHandler);
        EventBus.UnSubscribe<GameOverEvent>(GameOverHandler);
    }

    public void UpdateBossHealthBar(float value)
    {
        healthBar.value = value;
    }

    private void BossStartHandler(BossStartEvent evnt)
    {
        bossNameText.text = evnt.bossName;
        healthBar.value = 1.0f;
        healthBarUI.gameObject.SetActive(true);
    }

    private void BossClearHandler(BossClearEvent evnt)
    {
        healthBarUI.gameObject.SetActive(false);
    }

    private void GameOverHandler(GameOverEvent evnt)
    {
        healthBarUI.gameObject.SetActive(false);
    }
}
