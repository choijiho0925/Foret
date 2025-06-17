using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerSFX : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] jumpClip;
    public AudioClip[] attackMissClip;
    public AudioClip[] attackHitClip;
    public AudioClip[] throwAttackClip;
    public AudioClip[] dashClip;
    public AudioClip[] hitClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJumpClip()
    {
        int rand = Random.Range(0, jumpClip.Length);
        audioSource.PlayOneShot(jumpClip[rand]);
    }

    public void PlayAttackMissClip()
    {
        int rand = Random.Range(0, attackMissClip.Length);
        audioSource.PlayOneShot(attackMissClip[rand]);
    } 
    
    public void PlayAttackHitClip()
    {
        int rand = Random.Range(0, attackHitClip.Length);
        audioSource.PlayOneShot(attackHitClip[rand]);
    }

    public void PlayThrowAttackClip()
    {
        int rand = Random.Range(0, throwAttackClip.Length);
        audioSource.PlayOneShot(throwAttackClip[rand]);
    }
    
    public void PlayDashClip()
    {
        int rand = Random.Range(0, dashClip.Length);
        audioSource.PlayOneShot(dashClip[rand]);
    }   
    
    public void PlayHitClip()
    {
        int rand = Random.Range(0, hitClip.Length);
        audioSource.PlayOneShot(hitClip[rand]);
    }
}
