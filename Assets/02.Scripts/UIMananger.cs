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
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage();
        }

        if (Input.GetKeyDown(KeyCode.Q))
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