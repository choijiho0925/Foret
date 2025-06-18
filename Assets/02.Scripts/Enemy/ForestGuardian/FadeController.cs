using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public static FadeController Instance { get; private set; }

    public Image fadeImage;
    public float fadeDuration = 5f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void FadeOut(System.Action onComplete = null)
    {
        fadeImage.gameObject.SetActive(true);

        SetImageAlpha(0f);
        StartCoroutine(FadeRoutine(0f, 1f, onComplete));
    }

    public void FadeIn(System.Action onComplete = null)
    {
        fadeImage.gameObject.SetActive(true);
        SetImageAlpha(1f);
        StartCoroutine(FadeRoutine(1f, 0f, () =>
        {
            // 페이드 완료 후 비활성화
            fadeImage.gameObject.SetActive(false);
            onComplete?.Invoke();
        }));
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, System.Action onComplete)
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;

        onComplete?.Invoke();
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}