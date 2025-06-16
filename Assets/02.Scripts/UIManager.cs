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

        if (Input.GetKeyDown(KeyCode.M))
        {
            UpdateGauge(0.3f);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            UpdateGauge(-0.3f);
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