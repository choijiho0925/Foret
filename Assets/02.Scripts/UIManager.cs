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

    // Update is called once per frame
    void Update()
    {
        // 테스트용 키 입력
        if (Input.GetKeyDown(KeyCode.R))
        {
            TakeDamage();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Recovery();
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
}