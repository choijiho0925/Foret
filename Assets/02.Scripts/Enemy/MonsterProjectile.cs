using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;     // 투사체 이동 속도
    [SerializeField] private float lifeTime = 5f;   // 투사체 유지 시간
    [SerializeField] private GameObject projectile; // 실제 투사체

    private Vector3 direction;  // 투사체 방향
    private float damage;       // 투사체 데미지

    public void Initialize(Vector3 dir, float dmg)  // 방향과 데미지 받아오는 함수
    {
        direction = dir.normalized;
        damage = dmg;

        // 일정 시간 뒤 자동 제거
        Destroy(projectile, lifeTime);
        Destroy(gameObject, lifeTime + 2f);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어 데미지 처리
            IDamagable target = collision.GetComponent<IDamagable>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }

        Destroy(projectile);
        Destroy(gameObject, 2f);
    }
}
