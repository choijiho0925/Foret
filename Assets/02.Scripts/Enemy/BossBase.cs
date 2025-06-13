using System.Collections;
using UnityEngine;

public class BossBase : MonsterBase
{
    // 패턴 간 딜레이
    [SerializeField] protected float patternDelay = 2f;

    protected bool isPatterning = false;    // 현재 패턴 실행 중인지
    protected Coroutine currentPattern;     // 실행 중인 패턴 코루틴 참조

    private void Start()
    {
        base.Start();
        StartCoroutine(DetectPlayerRoutine());
    }

    // 일정 간격으로 플레이어 위치를 감지해, 사정 거리 내에 있으면 보스 패턴을 시작
    private IEnumerator DetectPlayerRoutine()
    {
        while(true)
        {
            // 플레이어와의 거리
            float distance = Vector3.Distance(transform.position, Player.transform.position);

            if(distance < DetectionRange && !isPatterning)
            {
                StartNextPattern();
            }

            // 0.2초마다 체크
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void StartNextPattern()
    {
        isPatterning = true;
        currentPattern = StartCoroutine(GoPattern());
    }

    protected virtual IEnumerator GoPattern()
    {
        yield return null;
        isPatterning = false;
    }

    protected void EndPattern()
    {
        isPatterning = false;
    }

    public override void Move()
    {
        
    }

    public override void Attack()
    {
        
    }
}
