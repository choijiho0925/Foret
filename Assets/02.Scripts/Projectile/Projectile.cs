using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileType projectileType;
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private bool isExplode;        //폭발 이팩트 연출 여부

    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 direction;
    private Coroutine returnCoroutine;

    private readonly int animIDHit = Animator.StringToHash("Hit");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        returnCoroutine = StartCoroutine(ReturnAfterDelay(lifeTime));
        SceneManager.sceneLoaded += OnSceneLoadedEvent;
    }

    private void OnDisable()
    {
        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
            returnCoroutine = null;
        }
        SceneManager.sceneLoaded -= OnSceneLoadedEvent;
        Reset();
    }

    public void Initialize(Vector3 dir, int dmg)
    {
        //총알 데미지, 방향 설정 후 발사
        damage = dmg;
        direction = dir;
        Fire();
    }

    private void Fire() //발사
    {
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }
    private void Reset()
    {
        //rb.position = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //데미지 처리 대상이고, IDamagable 객체일 떄만 데미지 처리
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0 &&
            collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.TakeDamage(damage);
        }

        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
        }

        if (isExplode)  //피격 이팩트 있을 시, 딜레이 후 풀에 반환
        {
            rb.velocity = Vector2.zero;
            animator.SetTrigger(animIDHit);
            returnCoroutine = StartCoroutine(ReturnAfterDelay(0.3f));
        }
        else
        {
            ReturnToPool();
        }
    }

    private IEnumerator ReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        PoolManager.Instance.ProjectilePool.Return(projectileType, this);
    }

    private void OnSceneLoadedEvent(Scene scene, LoadSceneMode mode)
    {   //Scene 전환 시 자동으로 비활성화
        ReturnToPool();
    }
}