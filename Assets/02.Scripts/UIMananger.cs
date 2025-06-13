using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMananger : Singleton<UIMananger>
{
    public HeartController heartController;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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