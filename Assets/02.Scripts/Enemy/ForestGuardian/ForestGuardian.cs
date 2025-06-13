using System.Collections;
using UnityEngine;

public class ForestGuardian : BossBase
{
    [Header("숲의 주인 설정")]
    [SerializeField] private float meleeRange = 2f;     // 근접 공격 거리
    [SerializeField] private float chaseRange = 5f;     // 달려오는 거리
    [SerializeField] private float teleportRange = 2f;  // 순간이동 거리

    // 초기 위치
    public Vector3 InitialPosition { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        InitialPosition = transform.position;

        // 초기 상태는 복귀
       
    }

    /// <summary>
    /// 가까우면 공격
    /// 중간 거리면 추격
    /// 멀면 텔레포트
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator ExecutePattern()
    {
        // 플레이어와의 거리
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        // 가까우면 공격
        if(distance <= meleeRange)
        {
            yield return MeleeAttack();
        }

        // 조금 떨어져 있으면 추격
        else if (distance <= chaseRange)
        {
            yield return ChasePlayer();
        }

        // 더 떨어져 있으면 텔레포트
        else if (distance <= DetectionRange)
        {
            yield return Teleport();
        }

        // 감지 범위 밖일 때
        else
        {
            Debug.Log("플레이어가 감지 범위 밖에 있으므로 복귀");
        }
    }

    // 근접 공격 코루틴
    private IEnumerator MeleeAttack()
    {
        Debug.Log("근접 공격 시도");

        Attack();
        yield return new WaitForSeconds(1f);
    }

    // 추격 코루틴
    private IEnumerator ChasePlayer()
    {
        Debug.Log("추격 시작");

        // 방향 계산
        Vector2 dir = (Player.transform.position - transform.position).normalized;

        // 이동 속도 적용
        transform.position += (Vector3)(dir * MoveSpeed * Time.deltaTime);

        yield return null;
    }

    // 텔레포트 코루틴
    private IEnumerator Teleport()
    {
        Debug.Log("순간이동");

        // 플레이어 위쪽 거리
        float checkDistance = 5f;

        // 위쪽 방향
        Vector2 teleportOffset = Vector2.up * checkDistance;

        // 보스 크기와 동일한 충돌 체크 영역
        Vector2 targetPos = (Vector2)Player.transform.position + teleportOffset;

        Collider2D collider = GetComponent<Collider2D>();

        if(collider == null)
        {
            Debug.LogWarning("보스 콜라이더 없음");
            yield break;
        }

        // 보스의 월드 크기
        Vector2 bossSize = collider.bounds.size;

        // 지형과 겹치는지 확인
        LayerMask mapMask = LayerMask.GetMask("Ground", "Top", "Wall");
        Collider2D hit = Physics2D.OverlapBox(targetPos, bossSize, 0f, mapMask);

        if (hit == null)
        {
            transform.position = targetPos;
            Debug.Log("순간이동 성공");
        }

        else
        {
            Debug.Log("순간이동 실패. 지형 충돌");
        }

        yield return new WaitForSeconds(1f);
    }
}
