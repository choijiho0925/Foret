using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private float delayTime = 0.5f; // 플레이어가 다리 위에 있을 때 지연 시간
    private float respawnTime = 5f; // 다리가 떨어진 후 재생성 시간
    private bool hasFallen; // 다리가 떨어졌는지 여부를 확인하는 변수
    private Vector3 startPosition; // 다리의 초기 위치
    private Rigidbody2D rb; // Rigidbody2D 컴포넌트

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position; // 다리의 초기 위치 저장
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !hasFallen)
        {
            StartCoroutine(Fall()); // 플레이어가 다리 위에 있을 때 다리가 떨어지도록 코루틴 시작
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeadZone"))
        {
            StartCoroutine(Respawn()); // 데드존에 닿으면 Respawn 메서드 호출
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(delayTime);

        rb.bodyType = RigidbodyType2D.Dynamic; // 다리를 동적 Rigidbody로 설정
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime); // 지정된 시간 동안 대기

        ResetSetting(); // 다리의 설정을 초기화하는 메서드 호출

    }

    private void ResetSetting()
    {
        rb.bodyType = RigidbodyType2D.Static; // 다리를 정적 Rigidbody로 설정
        transform.position = startPosition; // 다리의 위치를 초기 위치로 되돌림
    }
}
