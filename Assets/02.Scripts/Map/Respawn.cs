using UnityEngine;
using UnityEngine.InputSystem;

public class Respawn : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerStat playerStat;

    private Vector3 respawnPoint;
    private SaveSpawnPoint saveSpawnPoint; // SaveSpawnPoint 스크립트의 인스턴스

    public void ShowInteractUI()
    {

    }

    public void InteractAction()
    {

    }

    public void SetRespawnPoint(Vector3 newPoint) // 부활 지점을 설정하는 메소드
    {
        if (saveSpawnPoint.PlayerInZone)
        {
            respawnPoint = newPoint; // 새로운 부활 지점 설정
        }
    }

    public void SaveRespawnPoint() // 부활 지점을 저장하는 메소드
    {
        SetRespawnPoint(saveSpawnPoint.transform.position); // 현재 위치를 부활 지점으로 저장
    }

    public void RespawnPlayer() // 플레이어를 부활 지점으로 이동시키는 메소드
    {
        transform.position = respawnPoint; // 플레이어를 부활 지점으로 이동
        playerStat.TakeDamage(1);
        // 부활할때 체력 -1
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SavePoint"))
        {
            saveSpawnPoint = collision.GetComponent<SaveSpawnPoint>(); // SaveSpawnPoint 스크립트의 인스턴스를 가져옴
            Debug.Log("SaveSpawnPoint 영역에 들어옴"); // 디버그 메시지 출력
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("SavePoint"))
        {
            saveSpawnPoint = null; // SaveSpawnPoint 영역을 벗어나면 인스턴스 초기화
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("ChangeBackGroundZone")) // 다음 스테이지로 넘어가면 자동으로 부활지점을 다음 스테이지로 설정
        {
            respawnPoint = new Vector3(transform.position.x, transform.position.y - 18.5f, transform.position.z); // ChangeBackGroundZone 영역을 벗어나면 부활 지점을 초기화
        }
    }
}