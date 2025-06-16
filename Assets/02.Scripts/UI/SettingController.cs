using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public Button settingButton;
    public Button backButton;
    public GameObject settingPanel;
    public Slider totalSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        settingButton.onClick.AddListener(OnSettings);
        backButton.onClick.AddListener(OffSettings);
        UIManager.Instance.RegisterSettingController(this);
        AudioManager.Instance.RegisterTotalVolumeSlider(totalSlider);
        AudioManager.Instance.RegisterBGMSlider(bgmSlider);
        AudioManager.Instance.RegisterSFXSlider(sfxSlider);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSettings()
    {
        settingPanel.SetActive(true);
    }

    private void OffSettings()
    {
        settingPanel.SetActive(false);
    }
}