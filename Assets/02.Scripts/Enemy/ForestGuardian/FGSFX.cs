using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FGSFX : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] attackClip;
    public AudioClip backdownClip;
    public AudioClip teleportClip;
    //public AudioClip chargeClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAttackClip()
    {
        int rand = Random.Range(0, attackClip.Length);
        audioSource.PlayOneShot(attackClip[rand]);
    }

    public void PlayBackdownClip()
    {
        audioSource.PlayOneShot(backdownClip);
    }

    public void PlayTeleportClip()
    {
        audioSource.PlayOneShot(teleportClip);
    }

    //public void PlayChargeClip()
    //{
    //    audioSource.PlayOneShot(chargeClip);
    //}
}
