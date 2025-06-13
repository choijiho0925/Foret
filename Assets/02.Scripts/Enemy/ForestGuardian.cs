using System.Collections;
using UnityEngine;

public class ForestGuardian : BossBase
{
    [Header("숲의 주인 설정")]
    [SerializeField] private float meleeRange = 2f;         
    [SerializeField] private float teleportDistance = 2f;   

    protected override void Awake()
    {
        base.Awake();
    }

    protected override IEnumerator GoPattern()
    {
        // 범위 내에 있다면 공격, 없다면 순간이동
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        // 가까우면 공격
        if(distance <= meleeRange)
        {
            yield return MeleeAttack();
        }

        // 멀면 텔레포트
        else
        {
            yield return Teleport();
        }

        yield return new WaitForSeconds(patternDelay);
        EndPattern();
    }

    private IEnumerator MeleeAttack()
    {
        Debug.Log("근접 공격 시도");

        Attack();
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator Teleport()
    {
        Debug.Log("순간이동");

        // 플레이어 뒤쪽 방향 계산
        Vector3 toPlayer = (transform.position - Player.transform.position).normalized;
        Vector3 behindPosition = Player.transform.position + toPlayer * teleportDistance;

        // 순간이동
        transform.position = behindPosition;

        yield return new WaitForSeconds(1f);
    }
}
