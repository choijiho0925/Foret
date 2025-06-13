using UnityEngine;
using UnityEngine.InputSystem;

public class Respawn : MonoBehaviour
{
    private Vector3 respawnPoint;

    private SaveSpawnPoint saveSpawnPoint; // SaveSpawnPoint 스크립트의 인스턴스

    public void SetRespawnPoint(Vector3 newPoint) // 부활 지점을 설정하는 메소드
    {
        respawnPoint = newPoint; // 새로운 부활 지점 설정
    }

    public void SaveRespawnPoint() // 부활 지점을 저장하는 메소드
    {
        SetRespawnPoint(saveSpawnPoint.transform.position); // 현재 위치를 부활 지점으로 저장
    }

    public void OnSaveRespawn(InputAction.CallbackContext context) // InputAction을 통해 부활 지점을 저장하는 메소드
    {
        if (context.phase == InputActionPhase.Started && saveSpawnPoint.PlayerInZone)
        {
            SaveRespawnPoint(); // 플레이어가 영역에 있을 때 부활 지점을 저장하는 메소드 호출
        }
    }

    public void RespawnPlayer() // 플레이어를 부활 지점으로 이동시키는 메소드
    {
        transform.position = respawnPoint; // 플레이어를 부활 지점으로 이동
        // 부활할때 체력 -1 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SavePoint"))
        {
            saveSpawnPoint = collision.GetComponent<SaveSpawnPoint>(); // SaveSpawnPoint 스크립트의 인스턴스를 가져옴
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SavePoint"))
        {
            saveSpawnPoint = null; // SaveSpawnPoint 영역을 벗어나면 인스턴스 초기화
        }
    }
}