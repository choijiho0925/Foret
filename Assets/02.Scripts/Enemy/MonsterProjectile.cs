using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;     // 투사체 이동 속도
    [SerializeField] private float lifeTime = 5f;   // 투사체 유지 시간

    private Vector3 direction;  // 투사체 방향
    private int damage;       // 투사체 데미지

    public void Initialize(Vector3 dir, int dmg)  // 방향과 데미지 받아오는 함수
    {
        direction = dir.normalized;
        damage = dmg;
        Destroy(gameObject, lifeTime); // 일정 시간 뒤 자동 제거
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

        Destroy(gameObject);
    }
}
