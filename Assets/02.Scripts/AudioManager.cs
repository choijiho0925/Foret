using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        SetInitializeSlider();
    }

    private void SetInitializeSlider()
    {
        float masterVolume;
        float bgmVolume;
        float sfxVolume;

        audioMixer.GetFloat("Master", out masterVolume);
        audioMixer.GetFloat("BGM", out bgmVolume);
        audioMixer.GetFloat("SFX", out sfxVolume);

        masterSlider.value = Mathf.Pow(10, masterVolume / 20);
        bgmSlider.value = Mathf.Pow(10, bgmVolume / 20);
        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20);
    }

    public void SetMasterVolume(float sliderValue)
    {
        float volume = Mathf.Log10(sliderValue) * 20;

        audioMixer.SetFloat("Master", volume);
    }

    public void SetBGMVolume(float sliderValue)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSFXVolume(float sliderValue)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);
    }

    public void RegisterTotalVolumeSlider(Slider ms) => masterSlider = ms;
    public void RegisterBGMSlider(Slider bs) => bgmSlider = bs;
    public void RegisterSFXSlider(Slider ss) => sfxSlider = ss;
}
