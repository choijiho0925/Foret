using UnityEngine;

public class MonsterProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 5f;

    private Vector3 direction;
    private float damage;

    public void Initialize(Vector3 dir, float dmg)
    {
        direction = dir.normalized;
        damage = dmg;
        Destroy(gameObject, lifeTime); // 일정 시간 뒤 자동 제거(임시)
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

        }

        Destroy(gameObject);
    }
}
