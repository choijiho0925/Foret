using System.Collections;
using UnityEngine;

public class BossBase : MonsterBase
{
    // 패턴 간 딜레이
    [SerializeField] protected float patternDelay = 2f;

    protected bool isPatterning = false;
    protected Coroutine currentPattern;

    private void Start()
    {
        base.Start();
        StartCoroutine(DetectPlayerRoutine());
    }

    // 플레이어 발견 코루틴
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
