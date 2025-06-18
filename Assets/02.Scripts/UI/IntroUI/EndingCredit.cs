using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCredit : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private float scrollDistance;
    [SerializeField] private float duration;

    private void Start()
    {
        Vector2 startPos = content.anchoredPosition;
        Vector2 targetPos = startPos + Vector2.up * scrollDistance;

        content.DOAnchorPos(targetPos, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                StartCoroutine(EndSequence());
            });
    }

    IEnumerator EndSequence()
    {
        yield return new WaitForSeconds(2f);
        OnSkip();
    }

    public void OnSkip()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
