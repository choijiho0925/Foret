using System.Collections;
using UnityEngine;

public class BossBase : MonsterBase
{
    // ���� �� ������
    [SerializeField] protected float patternDelay = 2f;

    protected bool isPatterning = false;
    protected Coroutine currentPattern;

    private void Start()
    {
        base.Start();
        StartCoroutine(DetectPlayerRoutine());
    }

    // �÷��̾� �߰� �ڷ�ƾ
    private IEnumerator DetectPlayerRoutine()
    {
        while(true)
        {
            // �÷��̾���� �Ÿ�
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
