using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public bool IsTyping { get; private set; }

    private string fullCurrentLine;
    private Coroutine typingCoroutine;

    private DialogueNPC currentTarget;
    private string npcName;

    // 대화할 NPC를 타겟으로 설정
    public void SetTarget(DialogueNPC npc, string name)
    {
        currentTarget = npc;
        npcName = name;
    }

    // 타겟 클리어 메서드
    public void ClearTarget(DialogueNPC npc)
    {
        if (currentTarget == npc)
        {
            currentTarget = null;
        }
    }
    
    // 대화창 보여주는 메서드
    public void ShowDialoguePanel()
    {
        dialoguePanel.SetActive(true);
    }

    // 대사가 끝나고 대사창을 닫는 메서드
    public void HideDialoguePanel()
    {
        dialogueText.text = "";
        IsTyping = false;
        dialoguePanel.SetActive(false);
    }

    // 대사 출력이 끝난 후 호출할 메서드
    public void CompleteCurrentLineInstantly()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        dialogueText.text = fullCurrentLine;
        IsTyping = false;
    }

    // NPC의 대사를 보여주기 위한 메서드
    public void DisplayLine(string line)
    {
        npcNameText.text = npcName;
        line = line.Replace("\\n", "\n");
        fullCurrentLine = line;
        
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        
        ShowDialoguePanel();
        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    // NPC 대사를 한글자씩 출력하기 위한 IEnumerator
    private IEnumerator TypeLine(string line)
    {
        IsTyping = true;
        
        StringBuilder sb = new StringBuilder();
        dialogueText.text = "";
        foreach (char c in line)
        {
            sb.Append(c);
            dialogueText.text = sb.ToString();
            yield return new WaitForSeconds(0.05f);
        }
        
        IsTyping = false;
    }
}