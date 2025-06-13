using System.Collections;
using UnityEngine;

public class BossBase : MonsterBase
{
    // 패턴 간 딜레이
    [SerializeField] protected float patternDelay = 2f;

    // 플레이어가 범위 안에 들어왔는지
    private bool playerInRange = false;

    protected bool isPatterning = false;    // 현재 패턴 실행 중인지
    protected Coroutine currentPattern;     // 실행 중인 패턴 코루틴 참조

    private void Start()
    {
        base.Start();
    }

    // 플레이어가 사정 거리 내에 있으면 보스 패턴을 시작
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

            if(!isPatterning)
            {
                StartNextPattern();
            }
        }
    }

    // 플레이어가 사정 거리 내에서 벗어나면 보스 패턴 중지
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInRange = false;

            if(currentPattern != null)
            {
                StopCoroutine(currentPattern);
                currentPattern = null;
            }
            isPatterning = false;
        }
    }

    // 보스 패턴 실행 함수
    private void StartNextPattern()
    {
        isPatterning = true;
        currentPattern = StartCoroutine(GoPattern());
    }

    // 보스 패턴 실행 코루틴
    protected virtual IEnumerator GoPattern()
    {
        yield return null;
        isPatterning = false;
    }

    // 보스 패턴 종료
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
