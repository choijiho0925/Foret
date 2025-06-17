using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAttackHandler : MonoBehaviour
{
    private static readonly int isStart = Animator.StringToHash("IsStart");
    private static readonly int isEnd = Animator.StringToHash("IsEnd");

    [SerializeField] private int attackPower = 1;
    [SerializeField] private float attackDelay = 1.2f;
    [SerializeField] private float destroyDelay = 2f;
    [SerializeField] private Transform attackPos;
    [SerializeField] private LayerMask playerLayer;

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

        //Vector2 size = new Vector2(8f, 5.5f); // 캡슐 범위
        //float angle = 0f; // 수평 방향

        //Collider2D hit = Physics2D.OverlapCapsule(
        //    attackPos.position,
        //    size,
        //    CapsuleDirection2D.Horizontal,
        //    angle,
        //    playerLayer
        //);

        yield return new WaitForSeconds(attackDelay);

        animator.SetTrigger(isEnd);

        Destroy(gameObject, destroyDelay);
    }

    private void OnDrawGizmos() // 보스 공격 범위 (추후 삭제)
    {
        Vector2 size1 = new Vector2(3f, 6f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos.position, size1);
    }
}
