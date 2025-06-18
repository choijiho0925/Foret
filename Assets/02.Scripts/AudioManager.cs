using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Scene = UnityEngine.SceneManagement.Scene;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    public AudioSource bgmSource;   // MainBGM의 Audio Source 참조

    private Coroutine fadeCoroutine;
    private AudioClip beforeBGMClip;

    private Dictionary<string, string> bgmByScene = new Dictionary<string, string>()
    {
        { "TitleScene", "TitleBGM" },
        { "MainScene", "MainBGM" }
    };

    // Start is called before the first frame update
    void Start()
    {
        SetInitializeSlider();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[AudioManager] 씬 로드됨 : {scene.name}");
        if (scene.name == "MainScene")
        {
            Debug.Log("메인 씬 진입");
            // MainScene에 있는 "MainBGM" 오브젝트의 AudioSource를 자동 할당
            GameObject bgmObject = FindObjectInScene("MainBGM");
            if (bgmObject != null)
            {
                bgmSource = bgmObject.GetComponent<AudioSource>();
            }
            else
            {
                Debug.LogWarning("MainBGM 오브젝트를 찾을 수 없습니다.");
            }
        }
        else if (scene.name == "TitleScene")
        {
            Debug.Log("타이틀 씬 진입");
            GameObject bgmObject = FindObjectInScene("TitleBGM");
            if (bgmObject != null)
            {
                bgmSource = bgmObject.GetComponent<AudioSource>();
            }
            else
            {
                Debug.LogWarning("MainBGM 오브젝트를 찾을 수 없습니다.");
            }
        }
    }

    private GameObject FindObjectInScene(string name)
    {
        GameObject go = GameObject.Find(name);
        return go;
    }

    private void SetInitializeSlider()
    {
        if (audioMixer == null) return;

        if (masterSlider != null && audioMixer.GetFloat("Master", out float masterVolume))
            masterSlider.value = Mathf.Pow(10, masterVolume / 20);

        if (bgmSlider != null && audioMixer.GetFloat("BGM", out float bgmVolume))
            bgmSlider.value = Mathf.Pow(10, bgmVolume / 20);

        if (sfxSlider != null && audioMixer.GetFloat("SFX", out float sfxVolume))
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

    public void PlayBGM(AudioClip newClip, float fadeDuration = 1f, bool rememberPrevious = true)
    {
        if (rememberPrevious && bgmSource != null && bgmSource.clip != null)
        {
            beforeBGMClip = bgmSource.clip;
        }

        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeBGM(newClip, fadeDuration));
    }

    public void RestoreBeforeBGM(float fadeDuration = 1f)
    {
        if (beforeBGMClip != null)
        {
            PlayBGM(beforeBGMClip, fadeDuration, rememberPrevious: false);
        }
        else
        {
            Debug.LogWarning("복구할 원래 BGM이 없습니다.");
        }
    }

    private IEnumerator FadeBGM(AudioClip newClip, float duration)
    {
        float currentTime = 0f;
        float startVolume;

        audioMixer.GetFloat("BGM", out startVolume);
        startVolume = Mathf.Pow(10, startVolume / 20);

        // 페이드아웃
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
            audioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip = newClip;
        bgmSource.Play();

        // 페이드인
        currentTime = 0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float volume = Mathf.Lerp(0f, startVolume, currentTime / duration);
            audioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
            yield return null;
        }

        audioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Max(startVolume, 0.0001f)) * 20);
    }

    public void RegisterTotalVolumeSlider(Slider ms) => masterSlider = ms;
    public void RegisterBGMSlider(Slider bs) => bgmSlider = bs;
    public void RegisterSFXSlider(Slider ss) => sfxSlider = ss;
    public void RegisterBGMSource(AudioSource source) => bgmSource = source;
}
