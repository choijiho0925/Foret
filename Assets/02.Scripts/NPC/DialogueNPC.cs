using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private List<DialogueData> dialogueData;//스크립터블 오브젝트
    [SerializeField] private NpcController npcController;
    private int indexnum;
    private bool isDialogueStart;
    private PlayerInteract player;
    private Queue<string> dialogueQueue = new Queue<string>();//이거 프로텍티드 다른 사람한테 조금 물어보자
    private UIManager uiManager;

    private void Start()
    {
        player = FindObjectOfType<PlayerInteract>();
        uiManager = UIManager.Instance;
        isDialogueStart = true;
        indexnum = 0;//나중에 저장 만들 때 indexnum,npc위치, 상태 저장 =>각 상속받는 스크립트에서
    }

    public void ShowInteractUI()
    {
        uiManager.dialogueController.SetTarget(this, dialogueData[indexnum].npcName);
        uiManager.interactableController.ShowInteractable(this.gameObject.layer);
    }

    public void InteractAction()
    {
        uiManager.interactableController.HideInteractable();
        CheckAction();
    }

    private void CheckAction()
    {
        uiManager.dialogueController.IsScene(dialogueData[indexnum].isScene);
        if (dialogueData[indexnum].timing != ActionTiming.None)
        {
            npcController.SetTimeline(dialogueData[indexnum]);
            if (dialogueData[indexnum].timing == ActionTiming.Before)
            {
                npcController.action = InitDialogue;
                npcController.Playtimeline();
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
        if (indexnum >= dialogueData.Count) return;
        dialogueQueue.Clear();
        foreach (string dialogue in dialogueData[indexnum].dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }
    }

    // 말풍선 끄기
    

    private void ShowNextLine()
    {
        if (uiManager.dialogueController.IsTyping)
        {
            uiManager.dialogueController.CompleteCurrentLineInstantly();// 글자 다 안 나왔으면 바로 표시
            return;
        }

        if (dialogueQueue.Count == 0)
        {
            if (dialogueData[indexnum].isScene)
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
        uiManager.dialogueController.HideDialoguePanel();
        uiManager.dialogueController.ClearTarget(this);
        if (dialogueData[indexnum].timing == ActionTiming.After)
        {
            npcController.action = AfterTimeline;
            npcController.Playtimeline();
        }
        else
        {
            AfterTimeline();
        }
    }
    
    private void EndSpeechBubble()
    {
        uiManager.dialogueController.HideSpeechBubble();
        uiManager.dialogueController.ClearTarget(this);
        if (dialogueData[indexnum].timing == ActionTiming.After)
        {
            npcController.action = AfterTimeline;
            npcController.Playtimeline();
        }
        else
        {
            AfterTimeline();
        }
    }

    private void AfterTimeline()
    {
        isDialogueStart = true;
        if (dialogueData[indexnum].type == ActionType.Heal)
        {
            uiManager.interactableController.ShowInteractable(this.gameObject.layer);
        }
        indexnum++;//test용 indexnum를 높여주는 것은 퀘스트나 보스를 깼을 때 거기에 넣어주기
        player.OnEndInteraction();
    }
}
