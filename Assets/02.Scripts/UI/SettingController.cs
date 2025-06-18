using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public Button settingButton;
    public Button backButton;
    public Button controlButton;
    public Button controlBackButton;
    public GameObject settingPanel;
    public GameObject controlPanel;
    public Slider totalSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        settingButton.onClick.AddListener(OnSettings);
        backButton.onClick.AddListener(OffSettings);
        controlButton.onClick.AddListener(OnControl);
        controlBackButton.onClick.AddListener(OffControl);
        UIManager.Instance.RegisterSettingController(this);
        AudioManager.Instance.RegisterTotalVolumeSlider(totalSlider);
        AudioManager.Instance.RegisterBGMSlider(bgmSlider);
        AudioManager.Instance.RegisterSFXSlider(sfxSlider);
    }
    

    private void OnSettings()
    {
        settingPanel.SetActive(true);
    }

    private void OffSettings()
    {
        settingPanel.SetActive(false);
    }

    private void OnControl()
    {
        controlPanel.SetActive(true);
        settingPanel.SetActive(false); 
    }

    private void OffControl()
    {
        controlPanel.SetActive(false);
        settingPanel.SetActive(true);
    }
}