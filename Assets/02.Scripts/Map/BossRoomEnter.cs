using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomEnter : MonoBehaviour
{
    public string bossName;
    public AudioClip bossBGM;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventBus.Raise(new BossStartEvent(bossName, bossBGM));
        }
    }
}
