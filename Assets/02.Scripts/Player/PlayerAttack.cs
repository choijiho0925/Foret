using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace _02.Scripts.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public float dmgValue = 4;
        public GameObject ProjectilePrefab;
        
        private Rigidbody2D rb;
        private Animator animator;
        public bool canAttack = true;
        public bool isTimeToCheck = false;
        private Projectile projectile;

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

        [Header("원거리 공격 설정")] 
        public float throwPositionOffsetX = 0.5f;
        public float throwPositionOffsetY = 0.7f;
        

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
        }

        public void PerformAttack(Vector2 direction)
        {
            if (!canAttack) return; // 공격 불가능 상태면 즉시 리턴

            canAttack = false;
            
            // 위쪽 공격 (Y값이 특정 값 이상일 때)
            if (direction.y > 0.7f) 
            {
                Debug.Log("위쪽 공격!");
                animator.SetTrigger("IsAttackUp");
                // 위쪽 공격 로직 구현 
                DoDamage(attackPivotUp.position);
                StartCoroutine(ShowAttackEffect(attackEffectUp));
                
            }
            // 아래쪽 공격 (Y값이 특정 값 이하일 때 & 공중에 있을 때)
            else if (direction.y < -0.7f) 
            {
                Debug.Log("아래쪽 공격!");
                animator.SetTrigger("IsAttackDown");
                // 아래쪽 공격 로직 구현 
                StartCoroutine(ShowAttackEffect(attackEffectDown));
            }
            // 앞쪽 공격 (그 외 모든 경우)
            else
            {
                Debug.Log("앞쪽 공격!");
                animator.SetTrigger("IsAttack"); // 기본 공격 애니메이션
                // 여기에 앞쪽 공격 로직 구현 
                StartCoroutine(ShowAttackEffect(attackEffectForward));
            }
            StartCoroutine(AttackCooldown());
        }

        public void ThrowAttack()
        {
            Vector3 throwPositionOffset = new Vector3(throwPositionOffsetX * transform.localScale.x, throwPositionOffsetY * transform.localScale.y, 0);
            GameObject projectile = Instantiate(ProjectilePrefab, 
                transform.position + throwPositionOffset, Quaternion.identity) as GameObject; 
            Vector2 direction = new Vector2(transform.localScale.x, 0);
            projectile.GetComponent<Projectile>().direction = direction;    //추후 오브젝트 풀링으로 수정 예정
            projectile.name = "ThrowableWeapon";
        }
        // public void OnAttack(InputAction.CallbackContext context)
        // {
        //     if (canAttack && context.phase == InputActionPhase.Started)
        //     {
        //         canAttack = false;
        //         animator.SetTrigger("IsAttack");
        //         StartCoroutine(AttackCooldown());
        //     }
        // }   

        IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(0.25f);
            canAttack = true;
        }

        public void DoDamage(Vector2 attackPos)
        {
            dmgValue = Mathf.Abs(dmgValue);
            Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackPos, attackRadius, enemyLayer);

            for (int i = 0; i < collidersEnemies.Length; i++)
            {
                //데미지 처리
                
                // if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
                // {
                //     dmgValue = -dmgValue;
                // }
                //collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
            }
        }

        IEnumerator ShowAttackEffect(GameObject effect)
        {
            effect.SetActive(true);
            yield return new WaitForSeconds(0.2f); 
            effect.SetActive(false);
        }
    }
}