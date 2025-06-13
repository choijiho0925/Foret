using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveSpawnPoint : MonoBehaviour
{
    private bool playerInZone; // 플레이어가 영역에 있는지 여부를 나타내는 변수
    private GameObject player; // 플레이어 객체를 저장하는 변수

    public void OnSaveRespawn(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && playerInZone)
        {
            SaveRespawnPoint(); // 플레이어가 영역에 있을 때 부활 지점을 저장하는 메소드 호출
        }
    }

    public void SaveRespawnPoint() // 부활 지점을 저장하는 메소드
    {
        if (player != null)
        {
            Respawn respawn = player.GetComponent<Respawn>(); // Respawn 컴포넌트를 가져옴
            respawn.SetRespawnPoint(transform.position); // 현재 위치를 부활 지점으로 저장
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = true; // 플레이어가 영역에 들어왔음을 표시
            player = collision.gameObject; // 플레이어 객체를 저장
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = false; // 플레이어가 영역을 벗어났음을 표시
            player = null; // 플레이어가 영역을 벗어나면 player를 null로 설정
        }
    }
}
