using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    public HeartController heartController;
    public SettingController settingController;
    public DialogueController dialogueController;
    public InteractableController interactableController;
    public AbilityController abilityController;

    public TextMeshProUGUI gameStartText;

    private bool hasStarted = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            gameStartText.DOFade(0f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!dialogueController.speechBubble.activeSelf)
            {
                dialogueController.ShowSpeechBubble();
            }
            else
            {
                dialogueController.HideSpeechBubble();
            }
        }

        if (!hasStarted && Input.anyKeyDown)
        {
            hasStarted = true;
            EventBus.Raise(new GameStartEvent());
        }
    }

    public void TakeDamage()
    {
        heartController.RemoveHeart();
    }

    public void Recovery()
    {
        heartController.RecoverHeart();
    }

    public void UpdateGauge(float amount)
    {
        abilityController.UseGauge(amount);
    }
}