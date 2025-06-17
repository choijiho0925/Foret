using UnityEngine;
using System.Collections.Generic;

public class SaveSpawnPoint : MonoBehaviour, IInteractable
{
    public bool PlayerInZone => playerInZone; // 외부에서 플레이어가 영역에 있는지 확인할 수 있도록 프로퍼티로 노출
    public Material normalMaterial; // 일반 재질
    public Material outMaterial; // 플레이어가 영역에 있을 때 적용할 재질

    [SerializeField] private DialogueData saveTextData;
    
    private bool playerInZone; // 플레이어가 영역에 있는지 여부를 나타내는 변수
    private SpriteRenderer renderer; // 스프라이트 렌더러 컴포넌트
    private PlayerInteract player;
    private UIManager uiManager;
    private Queue<string> savePointQueue = new Queue<string>();

    private bool isStart;

    private void Start()
    {
        renderer = GetComponentInChildren<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 초기화
        player = FindAnyObjectByType<PlayerInteract>();
        uiManager = UIManager.Instance;
        isStart = true;
    }

    public void ShowInteractUI()
    {
        uiManager.dialogueController.SetTarget(this.gameObject, saveTextData.npcName);
        uiManager.interactableController.ShowInteractable(this.gameObject.layer);
    }

    public void InteractAction()
    {
        uiManager.interactableController.HideInteractable();
        uiManager.dialogueController.IsScene(saveTextData.isScene);
        InitInteraction();
    }
    
    private void InitInteraction()
    {
        if (isStart)
        {
            savePointQueue.Clear();
            foreach (string dialogue in saveTextData.dialogues)
            {
                savePointQueue.Enqueue(dialogue);
            }

            isStart = false;
        }
        ShowNextLine();
    }
    
    private void ShowNextLine()
    {
        if (savePointQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        string line = savePointQueue.Dequeue();
        uiManager.dialogueController.SetDialogue(line);//디알로그 출력
        uiManager.dialogueController.ShowDialoguePanel();
        uiManager.dialogueController.CompleteCurrentLineInstantly();// 글자 다 안 나왔으면 바로 표시
    }
    
    private void EndDialogue() //나중에 ESC키 같은 걸로 중간에 대사를 끊을 수 있을지도?
    {
        isStart = true;
        GameManager.Instance.SetRespawnPoint(this.transform.position);
        uiManager.dialogueController.ClearTarget(this.gameObject);
        uiManager.dialogueController.HideDialoguePanel();
        uiManager.interactableController.ShowInteractable(this.gameObject.layer);
        player.OnEndInteraction();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = true; // 플레이어가 영역에 들어왔음을 표시
            renderer.material = outMaterial; // 플레이어가 영역에 들어오면 outMaterial로 재질 변경
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = false; // 플레이어가 영역을 벗어났음을 표시
            renderer.material = normalMaterial; // 플레이어가 영역을 벗어나면 normalMaterial로 재질 변경
        }
    }
}