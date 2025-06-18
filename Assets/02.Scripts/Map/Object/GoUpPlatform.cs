using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GoUpPlatform : MonoBehaviour
{
    [SerializeField] GameObject platform; // 플랫폼 오브젝트
    [SerializeField] Transform playerTransform; // 플레이어의 Transform

    private float moveTime = 10f; // 플랫폼이 위로 올라가는 시간
    private float moveDistance = 53f; // 플랫폼이 올라가는 거리
    private bool isMoving; // 플랫폼이 현재 이동 중인지 여부
    private bool canMove = true; // 플랫폼이 이동 가능한 상태인지 여부

    private Vector3 originalPosition; // 플랫폼의 원래 위치
    private Transform platformTransform; // 플랫폼의 Transform

    private void Start()
    {
        platformTransform = platform.transform;
        originalPosition = platformTransform.position; // 플랫폼의 원래 위치 저장
    }

    private void Update()
    {
        ResetPosition();
    }

    private void ResetPosition()
    {
        if (playerTransform.position.y < transform.position.y && !canMove)
        {
            StartCoroutine(ResetPlatformPosition());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) // 플레이어가 플랫폼에 닿았을 때
        {
            if (playerTransform.position.y > transform.position.y && canMove && !isMoving)
            {
                collision.transform.SetParent(platform.transform); // 플레이어를 플랫폼의 자식으로 설정
                StartCoroutine(MoveUpPlatform()); // 플랫폼을 위로 이동시키는 코루틴 시작
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) // 플레이어가 플랫폼에서 벗어났을 때
        {
            if (collision.transform.parent == platform.transform)
            {
                collision.transform.SetParent(null); // 플레이어를 플랫폼의 자식에서 해제
            }
        }
    }

    private IEnumerator MoveUpPlatform()
    {
        isMoving = true;
        yield return new WaitForSeconds(1f);

        Vector3 targetPos = originalPosition + Vector3.up * moveDistance;
        yield return StartCoroutine(MoveToPosition(platformTransform, targetPos, moveTime));

        isMoving = false;
        canMove = false; // 플랫폼이 이동한 후에는 다시 이동할 수 없도록 설정
    }

    private IEnumerator ResetPlatformPosition()
    {
        isMoving = true;
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(MoveToPosition(platformTransform, originalPosition, 5f));

        isMoving = false;
        canMove = true; // 플랫폼이 원래 위치로 돌아오면 다시 이동할 수 있도록 설정
    }

    private IEnumerator MoveToPosition(Transform target, Vector3 destination, float duration)
    {
        Vector3 start = target.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            target.position = Vector3.Lerp(start, destination, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.position = destination;
    }
}