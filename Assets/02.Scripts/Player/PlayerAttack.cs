using System.Collections;
using UnityEngine;

namespace _02.Scripts.Player
{
    public class PlayerAttack : MonoBehaviour
    {
        public float dmgValue = 4;
        public GameObject throwableObject;
        public Transform attackCheck;
        private Rigidbody2D rb;
        private Animator animator;
        public bool canAttack = true;
        public bool isTimeToCheck = false;
        private Projectile projectile;

        public GameObject cam;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponentInChildren<Animator>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X) && canAttack)
            {
                canAttack = false;
                animator.SetBool("IsAttacking", true);
                StartCoroutine(AttackCooldown());
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                GameObject projectile = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f,-0.2f), Quaternion.identity) as GameObject; 
                Vector2 direction = new Vector2(transform.localScale.x, 0);
                projectile.GetComponent<Projectile>().direction = direction;    //추후 오브젝트 풀링으로 수정 예정
                projectile.name = "ThrowableWeapon";
            }
        }

        IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(0.25f);
            canAttack = true;
        }

        public void DoDashDamage()
        {
            dmgValue = Mathf.Abs(dmgValue);
            Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, 0.9f);
            for (int i = 0; i < collidersEnemies.Length; i++)
            {
                if (collidersEnemies[i].gameObject.tag == "Enemy")
                {
                    if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
                    {
                        dmgValue = -dmgValue;
                    }
                    collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
                }
            }
        }
    }
}