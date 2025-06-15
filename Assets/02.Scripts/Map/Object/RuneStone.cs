using System.Collections.Generic;
using UnityEngine;

public class RuneStone : MonoBehaviour, IInteractable
{
    public Material normalMaterial;
    public Material outLineMaterial;
    public GameObject interactGameObject; // 상호작용 오브젝트

    [SerializeField] private DialogueData explainData;
    
    private bool canGoNextStage = true; // 다음 스테이지로 넘어갈 수 있는지 여부
    private bool isPlayerInZone; // 플레이어가 영역에 있는지 여부
    private Renderer renderer;
    private UIManager uiManager;
    private Queue<string> runeStoneQueue = new Queue<string>();
    private PlayerInteract player;

    private void Start()
    {
        player = FindObjectOfType<PlayerInteract>();
        renderer = GetComponent<Renderer>();
        uiManager = UIManager.Instance;
        foreach (string dialogue in explainData.dialogues)
        {
            runeStoneQueue.Enqueue(dialogue);
        }
    }

    public void ShowInteractUI()
    {
        uiManager.interactableController.ShowInteractable(this.gameObject.layer);
    }

    public void InteractAction()
    {
        if (canGoNextStage && isPlayerInZone)
        {
            uiManager.interactableController.HideInteractable();
            ShowNextLine();
            OpenNextStage(); // 다음 스테이지로 넘어가는 메소드 호출
        }
    }

    private void OpenNextStage()
    {
        interactGameObject.SetActive(false); // 다음 스테이지로 넘어갈 수 있도록 상호작용 오브젝트 비활성화
    }
    
    private void ShowNextLine()
    {
        if (uiManager.dialogueController.IsTyping)
        {
            uiManager.dialogueController.CompleteCurrentLineInstantly();// 글자 다 안 나왔으면 바로 표시
            return;
        }

        if (runeStoneQueue.Count == 0)
        {
            EndDialogue();
        }
        string line = runeStoneQueue.Dequeue();
        uiManager.dialogueController.DisplayLine(line);//디알로그 출력
    }

    private void EndDialogue() //나중에 ESC키 같은 걸로 중간에 대사를 끊을 수 있을지도?
    {
        uiManager.dialogueController.HideDialoguePanel();
        uiManager.interactableController.ShowInteractable(this.gameObject.layer);
        player.OnEndInteraction();
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
