using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    private MonsterBase monster;
    private Collider2D hitboxCollider;

    private void Awake()
    {
        hitboxCollider = GetComponent<Collider2D>();
        monster = GetComponentInParent<MonsterBase>();

        // 기본 비활성화
        hitboxCollider.enabled = false;     
    }

    public void SetHitboxActive(bool isActive)
    {
        hitboxCollider.enabled = isActive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("플레이어 찾음");
            IDamagable damagable = collision.GetComponent<IDamagable>();

            if(damagable != null)
            {
                damagable.TakeDamage(monster.AttackPower);

                // 중복 타격 방지
                SetHitboxActive(false);
            }
        }
    }
}
