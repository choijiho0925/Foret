using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public Button settingButton;
    public Button backButton;
    public GameObject settingPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        settingButton.onClick.AddListener(OnSettings);
        backButton.onClick.AddListener(OffSettings);
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