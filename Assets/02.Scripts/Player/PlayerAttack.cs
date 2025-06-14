using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace _02.Scripts.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public float dmgValue = 4;
        private Rigidbody2D rb;
        private Animator animator;
        public bool canAttack = true;
        public bool isTimeToCheck = false;

        public GameObject cam;
        
        [Header("일반 공격 설정")]
        public LayerMask enemyLayer;
        public float attackRadius = 0.9f;
        public Transform attackPivotForward;
        public Transform attackPivotUp;
        public Transform attackPivotDown;
        public GameObject attackEffectForward;
        public GameObject attackEffectUp;
        public GameObject attackEffectDown;
        public GameObject attackHitEffect;  

        [Header("원거리 공격 설정")] 
        public float throwPositionOffsetX = 0.5f;
        public float throwPositionOffsetY = 0.7f;

        private static readonly int animIDAttackForward = Animator.StringToHash("IsAttack");
        private static readonly int animIDAttackUp = Animator.StringToHash("IsAttackUp");
        private static readonly int animIDAttackDown = Animator.StringToHash("IsAttackDown");
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
        }

        public void PerformAttack(Vector2 inputDirection)
        {
            if (!canAttack) return; // 공격 불가능 상태면 즉시 리턴

            canAttack = false;
            
            // 위쪽 공격 (Y값이 특정 값 이상일 때)
            if (inputDirection.y > 0.7f) 
            {
                animator.SetTrigger(animIDAttackUp);
                // 위쪽 공격 로직 구현 
                DoDamage(attackPivotUp.position);
                StartCoroutine(ShowAttackEffect(attackEffectUp));
                
            }
            // 아래쪽 공격 (Y값이 특정 값 이하일 때 & 공중에 있을 때)
            else if (inputDirection.y < -0.7f) 
            {
                animator.SetTrigger(animIDAttackDown);
                // 아래쪽 공격 로직 구현 
                DoDamage(attackPivotDown.position);
                StartCoroutine(ShowAttackEffect(attackEffectDown));
            }
            // 앞쪽 공격 (그 외 모든 경우)
            else
            {
                animator.SetTrigger(animIDAttackForward); // 기본 공격 애니메이션
                // 앞쪽 공격 로직 구현 
                DoDamage(attackPivotForward.position);
                StartCoroutine(ShowAttackEffect(attackEffectForward));
            }
            StartCoroutine(AttackCooldown());
        }

        public void ThrowAttack(Vector2 inputDirection)
        {
            //발사 위치 설정
            Vector3 throwPositionOffset = new Vector3(throwPositionOffsetX * transform.localScale.x, throwPositionOffsetY * transform.localScale.y, 0);
            Vector2 fireDirection;

            if (inputDirection.sqrMagnitude < 0.1f)
            {
                fireDirection = new Vector2(transform.localScale.x, 0);
            }
            else
            {
                fireDirection = inputDirection.normalized;
            }
            //발사 방향 설정
            float angle = Mathf.Atan2(fireDirection.y, fireDirection.x) * Mathf.Rad2Deg;
            
            
            PoolManager.Instance.ProjectilePool.Get(transform.position + throwPositionOffset, Quaternion.Euler(0, 0, angle));
        }

        IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(0.25f);
            canAttack = true;
        }

        public void DoDamage(Vector2 attackPos)
        {
            dmgValue = Mathf.Abs(dmgValue);
            //범위 내 모든 적 콜라이더를 자져옴
            Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackPos, attackRadius, enemyLayer);
    
            for (int i = 0; i < collidersEnemies.Length; i++)
            {
                // 정확한 피격 위치 계산 로직 

                // 현재 공격의 중심(attackPos)에서 가장 가까운 적의 콜라이더 표면 위치를 찾음
                Vector2 hitPoint = collidersEnemies[i].ClosestPoint(attackPos);

                // --- 이펙트 회전 로직 (이제 hitPoint를 기준으로 계산) ---
        

                // 3. 실제 피격 지점(hitPoint)에서 플레이어를 향하는 방향을 계산합니다.
                Vector2 inputDirection = (Vector3)attackPos - (Vector3)hitPoint;
                float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
                // 4. 이펙트를 적의 중심이 아닌, 계산된 실제 피격 지점(hitPoint)에 생성합니다.
                Instantiate(attackHitEffect, hitPoint, rotation);


                //데미지 처리 로직 
                //collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
            }
        }

        IEnumerator ShowAttackEffect(GameObject effect)
        {
            effect.SetActive(true);
            yield return new WaitForSeconds(0.2f); 
            effect.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            // attackPivot들이 Inspector에서 할당되었는지 확인하여 에러를 방지합니다.
            if (attackPivotForward == null || attackPivotUp == null || attackPivotDown == null)
            {
                return;
            }

            // --- 1. 앞쪽 공격 범위 (빨간색) ---
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPivotForward.position, attackRadius);

            // --- 2. 위쪽 공격 범위 (초록색) ---
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackPivotUp.position, attackRadius);

            // --- 3. 아래쪽 공격 범위 (파란색) ---
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPivotDown.position, attackRadius);
        }
    }
}