using UnityEngine;

public class SaveSpawnPoint : MonoBehaviour
{
    private bool playerInZone; // 플레이어가 영역에 있는지 여부를 나타내는 변수
    public bool PlayerInZone => playerInZone; // 외부에서 플레이어가 영역에 있는지 확인할 수 있도록 프로퍼티로 노출

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = true; // 플레이어가 영역에 들어왔음을 표시
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = false; // 플레이어가 영역을 벗어났음을 표시
        }
    }
}