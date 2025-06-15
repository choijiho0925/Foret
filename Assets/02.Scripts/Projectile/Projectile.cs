using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Rigidbody2D rb;
    

    private void OnEnable()
    {
        Invoke(nameof(SelfDestroy), lifeTime);
        SceneManager.sceneLoaded += OnSceneLoadedEvent;
    }

    private void OnDisable()
    {
        Reset();
        CancelInvoke(nameof(SelfDestroy));
        SceneManager.sceneLoaded -= OnSceneLoadedEvent;
    }

    public void Initialize(Vector3 dir, int dmg)
    {
        damage = dmg;
    }

    private void Fire() //발사
    {
        rb.AddForce(transform.right * speed);
    }
    private void Reset()
    {
        rb.position = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //데미지 처리 대상이고, IDamagable 객체일 떄만 데미지 처리
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0 &&
            collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.TakeDamage(damage);
        }
        ReturnToPool();
    }


    private void ReturnToPool()
    {
        //PoolManager.Instance.ProjectilePool.Return(this);
    }

    private void SelfDestroy()  //특정 시간이 지나면 자체 파괴(반환)
    {
        if (this.enabled)
        {
            ReturnToPool();
        }
    }

    private void OnSceneLoadedEvent(Scene scene, LoadSceneMode mode)
    {   //Scene 전환 시 자동으로 비활성화
        CancelInvoke(nameof(SelfDestroy));
        ReturnToPool();
    }
}