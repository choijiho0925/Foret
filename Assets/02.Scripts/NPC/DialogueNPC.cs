using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour
{
    protected Queue<string> dialogueLines = new Queue<string>();
    [SerializeField] protected DialogueData[] dialogueData;//스크립터블 오브젝트

    protected int indexnum;
    //ui참조

    //ui연결 필드 생성
    
    protected virtual void Start()
    {
        //ui연결 find추가
        indexnum = 0;//나중에 저장 만들 때 indexnum,npc위치, 상태 저장 =>각 상속받는 스크립트에서
    }

    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //UI에 상호작용 키 뜨는 함수
        }
    }



    private Queue<string> dialogueQueue = new Queue<string>();

    public virtual void StartDialogue()
    {
        if(indexnum >= dialogueLines.Count) return;
        
        dialogueQueue.Clear();
        foreach (string dialogue in dialogueData[indexnum].dialogues)
        {
            dialogueQueue.Enqueue(dialogue);
        }

        //dialogueUI.ShowDialoguePanel();
        ShowNextLine();
    }

    public virtual void ShowNextLine()
    {
        // if (dialogueUI.isTyping)
        // {
        //     dialogueUI.CompleteCurrentLineInstantly();// 글자 다 안 나왔으면 바로 표시
        //     return;
        // }

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string line = dialogueQueue.Dequeue();
        //dialogueUI.DisplayLine(line);//디알로그 출력
    }

    protected virtual void EndDialogue()
    {
        //dialogueUI.HideDialoguePanel();
    }
    //장훈님 밑에꺼 참고해서 만들면 될듯? 저거에 맞춰서 할게요
    
    // public class DialogueUI : MonoBehaviour
    // {
    //     public TextMeshProUIUG npcNameText;
    //     public TextMeshProUIUG dialogueText;
    //     public GameObject dialoguePanel;
    //     public bool isTyping{get;private set;}

    //     private string fullCurrentLine;
    //     private Coroutine typingCoroutine;
   
    //     public void ShowDialoguePanel()
    //     {
    //         dialoguePanel.SetActive(true);
    //     }
    //
    //     public void HideDialoguePanel()
    //     {
    //         dialogueText.text = "";
    //         dialoguePanel.SetActive(false);
    //     }
    
    //     public void CompleteCurrentLineInstantly()
    //     {
    //         if (typingCoroutine != null)
    //         {
    //             StopCoroutine(typingCoroutine);
    //         }
    //         dialogueText.text = fullCurrentLine;
    //         isTyping = false;
    //     }
    
    //     public void DisplayLine(string npcName, string line)
    //     {
    //         fullCurrentLine = line;
    //         npcNameText = npcName;
    
    //         if (typingCoroutine != null)
    //             StopCoroutine(typingCoroutine);
    //
    //         typingCoroutine = StartCoroutine(TypeLine(line));
    //     }
    
    //     private IEnumerator TypeLine(string line)
    //     {
    //         isTyping = true;
    //         dialogueText.text = "";
    //
    //         foreach (char c in line)
    //         {
    //             dialogueText.text += c;//==>요거 스트링빌더로 바꾸기!!
    //             yield return new WaitForSeconds(0.05f);
    //         }
    //
    //         isTyping = false;
    //     }
    // }
}
