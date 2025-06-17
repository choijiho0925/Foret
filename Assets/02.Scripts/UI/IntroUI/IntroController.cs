using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IntroController : MonoBehaviour
{
    [SerializeField] private Image introImage;
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private List<IntroTextData> introTextData;

    private int sceneIndex;
    private float duration = 1.5f;
    private Coroutine typingCoroutine;

    private void Start()
    {
        introImage.color = new Color(1, 1, 1, 0);
        sceneIndex = 0;
        DisplayLine(introTextData[sceneIndex].sprite,introTextData[sceneIndex].introText);
    }

    public void DisplayLine(Sprite sprite, string line)
    {
        line = line.Replace("\\n", "\n");
        introImage.sprite = sprite;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        introImage.DOFade(1.0f, duration);
        typingCoroutine = StartCoroutine(TypeLine(line));
    }
    
    private IEnumerator TypeLine(string line)
    {
        StringBuilder sb = new StringBuilder();
        introText.text = "";
        introText.color = new Color(1, 1, 1, 1);
        foreach (char c in line)
        {
            sb.Append(c);
            introText.text = sb.ToString();
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(2.0f);
        bool fadeComplete = false;
        introImage.DOFade(0.0f, duration).OnComplete(() => fadeComplete = true);
        introText.DOFade(0.0f, duration);

        yield return new WaitUntil(() => fadeComplete);

        sceneIndex++;
        if (sceneIndex < introTextData.Count)
        {
            DisplayLine(introTextData[sceneIndex].sprite, introTextData[sceneIndex].introText);
        }
        else
        {
            // 씬 전환
        }
    }
}
