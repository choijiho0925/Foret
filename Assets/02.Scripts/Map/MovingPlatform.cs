using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject platform; // 플랫폼 오브젝트
    [SerializeField] GameObject platform1; // 플랫폼 1 오브젝트
    [SerializeField] GameObject platform2; // 플랫폼 2 오브젝트
    [SerializeField] GameObject platform3; // 플랫폼 3 오브젝트

    private float moveDistance = 3f; // 플랫폼 이동거리
    private float moveSpeed = 2f; // 플랫폼 이동속도
    private float disappearTime = 1f; // 플랫폼 사라지는 시간
    private float respawnTime = 2f; // 플랫폼 재생성 시간
    private bool movingToTarget = true; // 플랫폼이 목표 위치로 이동 중인지 여부
    private Vector3 startPosition; // 플랫폼 시작 위치
    private Vector3 targetPosition; // 플랫폼 목표 위치
    private Transform player; // 플레이어 트랜스폼


    private void Start()
    {
        CheckDirection();
    }

    private void Update()
    {
        MovePlatform(); 
    }

    private void CheckDirection() // 플랫폼 이동 방향을 결정하는 메서드
    {
        startPosition = platform.transform.position; // 플랫폼의 시작 위치 저장

        bool isMoveDirection = Random.Range(0, 2) > 0; // 이동 방향 결정 (0 또는 1)
        float direction = isMoveDirection ? 1f : -1f; // 1이면 오른쪽, -1이면 왼쪽으로 이동
        targetPosition = startPosition + new Vector3(moveDistance * direction, 0, 0); // 목표 위치 설정
    }

    private void MovePlatform() // 플랫폼 이동 메서드 호출
    {
        Vector3 arrivalPoint = movingToTarget ? targetPosition : startPosition; // 현재 목표 위치 결정

        float fixedY = startPosition.y; // 플랫폼의 Y 좌표를 고정하기 위한 변수
        Vector3 currentPosition = platform.transform.position; // 현재 플랫폼 위치
        Vector3 targetFixedY = new Vector3(arrivalPoint.x, fixedY, arrivalPoint.z); // 목표 위치의 Y 좌표를 고정

        platform.transform.position = Vector3.MoveTowards(currentPosition, targetFixedY, moveSpeed * Time.deltaTime); // 플랫폼 이동

        if (Vector3.Distance(platform.transform.position, arrivalPoint) < 0.01f) // 목표 위치에 도달했는지 확인
        {
            movingToTarget = !movingToTarget; // 이동 방향 전환
        }
    }

    private void ClearParent() // 플레이어를 플랫폼의 자식에서 해제하는 메서드
    {
        if (player != null && player.parent == transform) // 플레이어가 플랫폼의 자식으로 설정되어 있는지 확인
        {
            player.SetParent(null); // 플레이어를 플랫폼의 자식에서 해제
            player = null; // 플레이어 트랜스폼 초기화
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) // 플레이어 레이어 확인
        {
            player = collision.transform; // 플레이어 트랜스폼 저장
            StartCoroutine(SetPlayerAsChildNextFrame());
            StartCoroutine(Respawn()); // 플레이어가 플랫폼에 닿으면 재생성 코루틴 시작
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) // 플레이어가 플랫폼에서 벗어났을 때
        {
            ClearParent(); // 플레이어를 플랫폼의 자식에서 해제
        }
    }

    private IEnumerator SetPlayerAsChildNextFrame()
    {
        yield return null; // 한 프레임 대기
        player.SetParent(transform);
    }

    private IEnumerator Respawn() // 플랫폼 재생성 코루틴
    {
        yield return new WaitForSeconds(disappearTime); // 플랫폼 사라지는 시간 동안 대기

        ClearParent(); // 플레이어를 플랫폼의 자식에서 해제

        EnablePlatform(); // 플랫폼 비활성화 메서드 호출

        platform.transform.position = startPosition; // 플랫폼 위치를 시작 위치로 되돌림

        yield return new WaitForSeconds(respawnTime); // 지정된 시간 동안 대기

        OnablePlatform(); // 플랫폼 활성화 메서드 호출
    }

    private void EnablePlatform()
    {
        platform1.GetComponentInChildren<SpriteRenderer>().enabled = false; // 플랫폼 렌더러 비활성화
        platform1.GetComponentInChildren<Collider2D>().enabled = false; // 플랫폼 충돌체 비활성화
        platform2.GetComponentInChildren<SpriteRenderer>().enabled = false; // 플랫폼 렌더러 비활성화
        platform2.GetComponentInChildren<Collider2D>().enabled = false; // 플랫폼 충돌체 비활성화
        platform3.GetComponentInChildren<SpriteRenderer>().enabled = false; // 플랫폼 렌더러 비활성화
        platform3.GetComponentInChildren<Collider2D>().enabled = false; // 플랫폼 충돌체 비활성화
    }

    private void OnablePlatform()
    {
        platform1.GetComponentInChildren<SpriteRenderer>().enabled = true; // 플랫폼 렌더러 비활성화
        platform1.GetComponentInChildren<Collider2D>().enabled = true; // 플랫폼 충돌체 비활성화
        platform2.GetComponentInChildren<SpriteRenderer>().enabled = true; // 플랫폼 렌더러 비활성화
        platform2.GetComponentInChildren<Collider2D>().enabled = true; // 플랫폼 충돌체 비활성화
        platform3.GetComponentInChildren<SpriteRenderer>().enabled = true; // 플랫폼 렌더러 비활성화
        platform3.GetComponentInChildren<Collider2D>().enabled = true; // 플랫폼 충돌체 비활성화
    }
}