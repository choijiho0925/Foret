using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public HeartController heartController;
    public SettingController settingController;
    public DialogueController dialogueController;
    public InteractableController interactableController;
    public AbilityController abilityController;

    private bool hasStarted = false;

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