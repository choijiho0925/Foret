using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicPlant : MonoBehaviour,IInteractable
{
    [SerializeField] private DialogueData explainData;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material outLineMaterial;
    
    private PlayerInteract player;
    private GameManager gameManager;
    private UIManager uiManager;
    private Queue<string> plantQueue = new Queue<string>();
    private bool isPlayerInZone; // 플레이어가 영역에 있는지 여부
    private Renderer renderer;
    

    private void Start()
    {
        player = FindObjectOfType<PlayerInteract>();
        renderer = GetComponent<Renderer>();
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        foreach (string dialogue in explainData.dialogues)
        {
            plantQueue.Enqueue(dialogue);
        }
    }
    
    public void ShowInteractUI()
    {
        uiManager.interactableController.ShowInteractable(this.gameObject.layer);
        uiManager.dialogueController.SetTarget(this.gameObject, explainData.npcName);
    }
    
    public void InteractAction()
    {
        uiManager.interactableController.HideInteractable();
        uiManager.dialogueController.IsScene(explainData.isScene);
        ShowNextLine();
    }
    
    private void ShowNextLine()
    {
        if (plantQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        string line = plantQueue.Dequeue();
        uiManager.dialogueController.SetDialogue(line);//디알로그 출력
        uiManager.dialogueController.ShowDialoguePanel();
        uiManager.dialogueController.CompleteCurrentLineInstantly();// 글자 다 안 나왔으면 바로 표시
    }
    
    private void EndDialogue() //나중에 ESC키 같은 걸로 중간에 대사를 끊을 수 있을지도?
    {
        uiManager.dialogueController.ClearTarget(this.gameObject);
        GetPlant();
        uiManager.dialogueController.HideDialoguePanel();
        player.OnEndInteraction();
    }

    private void GetPlant()
    {
        gameManager.NextIndex();
        this.gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerInZone = true; // 플레이어가 영역에 들어옴
            renderer.material = outLineMaterial; // 플레이어가 영역에 들어오면 아웃라인 머티리얼로 변경
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerInZone = false; // 플레이어가 영역을 벗어남
            renderer.material = normalMaterial; // 플레이어가 영역을 벗어나면 원래 머티리얼로 변경
        }
    }
}
