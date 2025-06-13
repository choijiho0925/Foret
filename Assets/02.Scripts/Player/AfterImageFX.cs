using UnityEngine;
using System.Collections;

//잔상 효과 전용 클래스
public class AfterimageFX : MonoBehaviour   
{
    [SerializeField] private float fadeOutTime = 0.5f; // 사라지는 데 걸리는 시간

    private SpriteRenderer spriteRenderer;
    private Color startColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // 현재 색상 저장
        startColor = spriteRenderer.color;
        
        // 서서히 사라지는 코루틴 시작
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        
        // fadeOutTime 동안 반복
        while (timer < fadeOutTime)
        {
            // 경과 시간에 따라 알파(투명도) 값을 1에서 0으로 변경
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeOutTime);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            timer += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 완전히 사라지면 오브젝트 파괴
        Destroy(gameObject);
    }
}