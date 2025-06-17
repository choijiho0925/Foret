using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : Singleton<UIManager>
{
    [Header("Controller Prefabs")]
    [SerializeField] private HeartController heartControllerPF;
    [SerializeField] private SettingController settingControllerPF;
    [SerializeField] private DialogueController dialogueControllerPF;
    [SerializeField] private InteractableController interactableControllerPF;
    [SerializeField] private AbilityController abilityControllerPF;

    public HeartController heartController;
    public SettingController settingController;
    public DialogueController dialogueController;
    public InteractableController interactableController;
    public AbilityController abilityController;

    public TextMeshProUGUI gameStartText;
    private bool hasStarted = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "TitleScene")
        {
            gameStartText.DOFade(0f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!dialogueController.speechBubble.activeSelf)
            {
                dialogueController.ShowSpeechBubble();
            }
            else
            {
                dialogueController.HideSpeechBubble();
            }
        }

        if (!hasStarted && Input.anyKeyDown)
        {
            hasStarted = true;
            EventBus.Raise(new GameStartEvent());
        }
    }

    public void TakeDamage()
    {
        heartController.RemoveHeart();
    }

    public void Recovery()
    {
        heartController.RecoverHeart();
    }
    
    public void UpdateGauge(float amount)
    {
        abilityController.UpdateGauge(amount);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            InstantiateControllers();
        }
    }

    private void InstantiateControllers()
    {
        Instantiate(dialogueControllerPF);
        Instantiate(heartControllerPF);
        Instantiate(abilityControllerPF);
        Instantiate(interactableControllerPF);
        Instantiate(settingControllerPF);
    }

    public void RegisterHeartController(HeartController hc) => heartController = hc;
    public void RegisterSettingController(SettingController sc) => settingController = sc;
    public void RegisterDialogueController(DialogueController dc) => dialogueController = dc;
    public void RegisterInteractableController(InteractableController ic) => interactableController = ic;
    public void RegisterAbilityController(AbilityController ac) => abilityController = ac;
}