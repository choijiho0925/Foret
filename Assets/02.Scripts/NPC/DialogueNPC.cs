using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private List<DialogueData> dialogueData;//스크립터블 오브젝트
    [SerializeField] private NpcController npcController;

    private bool isDialogueStart;

    private PlayerInteract player;
    private Queue<string> dialogueQueue = new Queue<string>();//이거 프로텍티드 다른 사람한테 조금 물어보자
    private UIManager uiManager;
    private GameManager gameManager;
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        player = FindObjectOfType<PlayerInteract>();
        if (dialogueData.Count > 1)
        {
            virtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        uiManager = UIManager.Instance;
        gameManager = GameManager.Instance;
        isDialogueStart = true;
        //나중에 저장 만들 때 indexnum,npc위치, 상태 저장 =>각 상속받는 스크립트에서
    }

    public void ShowInteractUI()
    {
        int index = (dialogueData.Count == 1) ? 0 : gameManager.mainNpcIndex;
        if (!npcController.canInteract)
        {
            player.OnEndInteraction();
            return;
        }

        uiManager.dialogueController.SetTarget(this.gameObject, dialogueData[index].npcName);
        uiManager.interactableController.ShowInteractable(this.gameObject.layer);
    }

    public void InteractAction()
    {
        int index = (dialogueData.Count == 1) ? 0 : gameManager.mainNpcIndex;
        if (!npcController.canInteract)
        {
            player.OnEndInteraction();
            return;
        }

        if (dialogueData[index].isScene)
        {
            virtualCamera.Priority = 15;
        }

        uiManager.interactableController.HideInteractable();
        CheckAction();
    }

    private void CheckAction()
    {
        int index = (dialogueData.Count == 1) ? 0 : gameManager.mainNpcIndex;
        uiManager.dialogueController.IsScene(dialogueData[index].isScene);
        if (dialogueData[index].timing != ActionTiming.None)
        {
            npcController.SetTimeline(dialogueData[index]);
            if (dialogueData[index].timing == ActionTiming.Before)
            {
                npcController.action = InitDialogue;
                npcController.PlayTimeline();
            }
            else
            {
                InitDialogue();
            }
        }
        else
        {
            InitDialogue();
        }
    }

    private void InitDialogue()
    {
        if (isDialogueStart)
        {
            StartDialogue();
            isDialogueStart = false;
        }
        ShowNextLine();
    }

    private void StartDialogue()
    {
        int index = (dialogueData.Count == 1) ? 0 : gameManager.mainNpcIndex;
        if (index >= dialogueData.Count) return;
        dialogueQueue.Clear();
        foreach (string dialogue in dialogueData[index].dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }
    }

    private void ShowNextLine()
    {
        int index = (dialogueData.Count == 1) ? 0 : gameManager.mainNpcIndex;
        if (uiManager.dialogueController.IsTyping)
        {
            uiManager.dialogueController.CompleteCurrentLineInstantly();// 글자 다 안 나왔으면 바로 표시
            return;
        }

        if (dialogueQueue.Count == 0)
        {
            if (dialogueData[index].type == ActionType.Attack)
            {
                npcController.canInteract = false;
            }
            if (dialogueData[index].isScene)
            {
                EndSpeechBubble();
            }
            else
            {
                EndDialogue();
            }
            return;
        }

        string line = dialogueQueue.Dequeue();
        uiManager.dialogueController.DisplayLine(line);//디알로그 출력
    }

    private void EndDialogue()//나중에 ESC키 같은 걸로 중간에 대사를 끊을 수 있을지도?
    {
        int index = (dialogueData.Count == 1) ? 0 : gameManager.mainNpcIndex;
        uiManager.dialogueController.HideDialoguePanel();
        uiManager.dialogueController.ClearTarget(this.gameObject);
        if (dialogueData[index].timing == ActionTiming.After)
        {
            npcController.action = AfterTimeline;
            npcController.PlayTimeline();
        }
        else
        {
            AfterTimeline();
        }
    }

    private void EndSpeechBubble()
    {
        int index = (dialogueData.Count == 1) ? 0 : gameManager.mainNpcIndex;
        uiManager.dialogueController.HideSpeechBubble();
        uiManager.dialogueController.ClearTarget(this.gameObject);
        if (dialogueData[index].timing == ActionTiming.After)
        {
            npcController.action = AfterTimeline;
            npcController.PlayTimeline();
        }
        else
        {
            AfterTimeline();
        }
    }

    private void AfterTimeline()
    {
        int index = (dialogueData.Count == 1) ? 0 : gameManager.mainNpcIndex;
        isDialogueStart = true;
        if (dialogueData[index].type == ActionType.Heal)
        {
            uiManager.interactableController.ShowInteractable(this.gameObject.layer);
        }
        player.OnEndInteraction();
        if (dialogueData[index].timing != ActionTiming.None)
        {
            virtualCamera.Priority = 3;
        }
    }
}
