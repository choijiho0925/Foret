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
    public TextMeshProUGUI speechBubbleText;

    // NPC의 말풍선을 띄우기 위한 멤버변수들
    public Transform target;    // 타겟 NPC 위치
    public Vector3 offset = new Vector3(0f, 3f, 0f);      // NPC 머리 위 위치 조절
    public GameObject speechBubble; // 말풍선
    public RectTransform rect; // 말풍선 Rect

    public bool IsTyping { get; private set; }

    private string fullCurrentLine;
    private Coroutine typingCoroutine;

    private GameObject currentTarget;
    private string npcName;

    private bool isScene;

    private void Start()
    {
        UIManager.Instance.RegisterDialogueController(this);
    }

    // 대화할 NPC를 타겟으로 설정
    public void SetTarget(GameObject gameObject, string name)
    {
        currentTarget = gameObject;
        npcName = name;
        target = gameObject.transform;
    }

    // 타겟 클리어 메서드
    public void ClearTarget(GameObject gameObject)
    {
        if (currentTarget == gameObject)
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

    public void ShowSpeechBubble()
    {
        if (target == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);
        rect.position = screenPos;

        speechBubble.SetActive(true);
    }

    public void HideSpeechBubble()
    {
        if (speechBubble != null)
        {
            speechBubble.SetActive(false);
        }
        speechBubbleText.text = "";
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

        if (isScene)
        {
            speechBubbleText.text = fullCurrentLine;
        }
        else
        {
            dialogueText.text = fullCurrentLine;
        }
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

        if (isScene)
        {
            ShowSpeechBubble();
        }
        else
        {
            ShowDialoguePanel();
        }
        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    public void SetDialogue(string line)
    {
        npcNameText.text = npcName;
        line = line.Replace("\\n", "\n");
        fullCurrentLine = line;
    }

    // NPC 대사를 한글자씩 출력하기 위한 IEnumerator
    private IEnumerator TypeLine(string line)
    {
        IsTyping = true;

        StringBuilder sb = new StringBuilder();
        if (isScene)
        {
            speechBubbleText.text = "";
        }
        else
        {
            dialogueText.text = "";
        }
        foreach (char c in line)
        {
            sb.Append(c);
            if (isScene)
            {
                speechBubbleText.text = sb.ToString();
            }
            else
            {
                dialogueText.text = sb.ToString();
            }
            yield return new WaitForSeconds(0.05f);
        }

        IsTyping = false;
    }

    public void IsScene(bool b)
    {
        isScene = b;
    }
}