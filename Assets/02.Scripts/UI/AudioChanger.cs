using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChanger : MonoBehaviour
{
    public AudioClip bossBGM;
    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasPlayed) return;

        if (collision.CompareTag("Player"))
        {
            //AudioManager.Instance.PlayBGM();
        }
    }
}
