using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChanger : MonoBehaviour
{
    public AudioClip newBGM;
    private bool hasPlayed = false;

    public void PlayFirstBossBGM()
    {
       //AudioManager.Instance.PlayBGM();
       AudioManager.Instance.PlayBGM(newBGM, 1f);
       hasPlayed = true;
    }
}
