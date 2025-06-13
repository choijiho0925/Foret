using System.Collections;
using UnityEngine;

public class ChangeBackGround : MonoBehaviour
{
    [SerializeField] GameObject firstBackGroundObj;
    [SerializeField] GameObject secondBackGround1Obj;
    [SerializeField] GameObject secondBackGround2Obj;

    private SpriteRenderer firstBackGround; // 첫 번째 배경 스프라이트 렌더러
    private SpriteRenderer secondBackGround1; // 두 번째1 배경 스프라이트 렌더러
    private SpriteRenderer secondBackGround2; // 두 번째2 배경 스프라이트 렌더러

    private float fadeDuration = 3f; // 페이드 아웃/인 시간
    private float fadeTime; // 현재 페이드 시간

    private void Start()
    {
        firstBackGround = firstBackGroundObj.GetComponent<SpriteRenderer>();
        secondBackGround1 = secondBackGround1Obj.GetComponent<SpriteRenderer>();
        secondBackGround2 = secondBackGround2Obj.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ChangeBackgroundCoroutine());
        }
    }

    private IEnumerator ChangeBackgroundCoroutine()
    {
        Color firstColor = firstBackGround.color; // 첫 번째 배경 색상 저장

        while (fadeTime < fadeDuration) // 페이드 아웃 시간 동안 반복
        {
            float time = Mathf.Clamp01(fadeTime / fadeDuration); // 시간 비율 계산 (0~1 사이)
            float alpha = Mathf.Lerp(firstColor.a, 0f, time); // 알파 값 계산
            firstBackGround.color = new Color(firstColor.r, firstColor.g, firstColor.b, alpha); // 첫 번째 배경 색상 업데이트
            fadeTime += Time.deltaTime; // 시간 증가
            yield return null; // 다음 프레임까지 대기
        }
        
        Color secondColor1 = secondBackGround1.color; // 두 번째1 배경 색상 저장
        Color secondColor2 = secondBackGround2.color; // 두 번째2 배경 색상 저장

        fadeTime = 0f; // 페이드 시간 초기화

        while (fadeTime < fadeDuration)
        {
            float time = Mathf.Clamp01(fadeTime / fadeDuration); // 시간 비율 계산 (0~1 사이)
            float alpha = Mathf.Lerp(0f, 1f, time); // 알파 값 계산
            secondBackGround1.color = new Color(secondColor1.r, secondColor1.g, secondColor1.b, alpha); // 두 번째1 배경 색상 업데이트
            secondBackGround2.color = new Color(secondColor2.r, secondColor2.g, secondColor2.b, alpha); // 두 번째2 배경 색상 업데이트
            fadeTime += Time.deltaTime; // 시간 증가
            yield return null; // 다음 프레임까지 대기
        }
    }
}