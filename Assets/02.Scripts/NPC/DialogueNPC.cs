using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData[] dialogueData;//스크립터블 오브젝트
    private PlayerInteract player;
    private int indexnum;
    private Queue<string> dialogueQueue = new Queue<string>();//이거 프로텍티드 다른 사람한테 조금 물어보자
    private bool isDialogueStart;
    
    private void Start()
    {
        indexnum = 0;//나중에 저장 만들 때 indexnum,npc위치, 상태 저장 =>각 상속받는 스크립트에서
        player = FindObjectOfType<PlayerInteract>();
    }

    public virtual void ShowInteractUI()
    {
        //ui매니저에서 상호작용 연결
        isDialogueStart = true;
        UIManager.Instance.dialogueController.SetTarget(this, dialogueData[indexnum].npcName);
    }

    public virtual void InteractAction()
    {
        Debug.Log("StartInteract");
        if (isDialogueStart)
        {
            StartDialogue();
            isDialogueStart = false;
        }
        ShowNextLine();
    }

    private void StartDialogue()
    {
        Debug.Log("StartDialogue");
        if(indexnum >= dialogueData.Length) return;
        Debug.Log("StartDialogue");
        dialogueQueue.Clear();
        foreach (string dialogue in dialogueData[indexnum].dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }
    }

    private void ShowNextLine()
    {
        if (UIManager.Instance.dialogueController.IsTyping)
        {
            UIManager.Instance.dialogueController.CompleteCurrentLineInstantly();// 글자 다 안 나왔으면 바로 표시
            return;
        }

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string line = dialogueQueue.Dequeue();
        UIManager.Instance.dialogueController.DisplayLine(line);//디알로그 출력
    }

    private void EndDialogue()//나중에 ESC키 같은 걸로 중간에 대사를 끊을 수 있을지도?
    {
        UIManager.Instance.dialogueController.HideDialoguePanel();
        UIManager.Instance.dialogueController.ClearTarget(this);
        indexnum++;//test용 indexnum를 높여주는 것은 퀘스트나 보스를 깼을 때 거기에 넣어주기
        player.OnEndInteraction();
    }
}
