using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAttackHandler : MonoBehaviour
{
    private static readonly int isStart = Animator.StringToHash("IsStart");
    private static readonly int isEnd = Animator.StringToHash("IsEnd");

    [SerializeField] private int attackPower = 1;       // 공격력
    [SerializeField] private float attackDelay = 1.2f;  // 소환된 후 공격 딜레이
    [SerializeField] private float destroyDelay = 2f;   // 공격 후 파괴 딜레이
    [SerializeField] private Transform attackPos;       // 공격 위치
    [SerializeField] private LayerMask playerLayer;     // 공격을 위한 레이어

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        StartCoroutine(DropAttackSequence());
    }

    private IEnumerator DropAttackSequence()
    {
        animator.SetTrigger(isStart);

        yield return new WaitForSeconds(attackDelay);

        Vector2 size = new Vector2(5f, 11f); // 캡슐 범위
        float angle = 0f; // 수평 방향

        Collider2D hit = Physics2D.OverlapCapsule(
            attackPos.position,
            size,
            CapsuleDirection2D.Horizontal,
            angle,
            playerLayer
        );

        if (hit != null)
            hit.GetComponent<IDamagable>()?.TakeDamage(attackPower);

        yield return new WaitForSeconds(attackDelay);

        animator.SetTrigger(isEnd);

        Destroy(gameObject, destroyDelay);
    }

    private void OnDrawGizmos() // 보스 공격 범위 (추후 삭제)
    {
        Vector2 size1 = new Vector2(5f, 11f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, size1);
    }
}
