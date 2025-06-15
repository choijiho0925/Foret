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
        private PlayerStat playerStat;
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
            playerStat = GetComponent<PlayerStat>();
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
            if (!canAttack) return;
            canAttack = false;
            
            if (inputDirection.y > 0.7f) 
            {
                animator.SetTrigger(animIDAttackUp); 
                
            }
            else if (inputDirection.y < -0.7f) 
            {
                animator.SetTrigger(animIDAttackDown); 
            }
            else
            {
                animator.SetTrigger(animIDAttackForward); 
            }
            animator.SetTrigger(animIDAttackForward); 
            
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
            
            StartCoroutine(AttackCooldown());
        }

        IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(0.25f);
            canAttack = true;
        }

        public void DoDamage(Vector2 attackPos)
        {
            //범위 내 모든 적 콜라이더를 자져옴
            Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackPos, attackRadius, enemyLayer);
    
            for (int i = 0; i < collidersEnemies.Length; i++)
            {
                // 정확한 피격 위치 계산 로직 
                Vector2 hitPoint = collidersEnemies[i].ClosestPoint(attackPos);
                Vector2 inputDirection = (Vector3)attackPos - (Vector3)hitPoint;
                float angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
        
                Instantiate(attackHitEffect, hitPoint, rotation);

                //데미지 처리 로직 
                if (collidersEnemies[i].TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    damagable.TakeDamage(playerStat.CurrentAttackDamage);
                }
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
            if (attackPivotForward == null || attackPivotUp == null || attackPivotDown == null)
            {
                return;
            }

            // 1. 앞쪽 공격 범위 (빨간색) 
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPivotForward.position, attackRadius);

            // 2. 위쪽 공격 범위 (초록색) 
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(attackPivotUp.position, attackRadius);

            // 3. 아래쪽 공격 범위 (파란색) 
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPivotDown.position, attackRadius);
        }
    }
}