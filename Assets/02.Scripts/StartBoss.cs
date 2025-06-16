using UnityEngine;

public class StartBoss : MonoBehaviour
{
    [SerializeField] private GameObject bossRoomCollider;

    private void Start()
    {
        bossRoomCollider.SetActive(false);
    }

    public void StartBossRoom()
    {
        bossRoomCollider.SetActive(true);
        GameManager.Instance.SetRespawnPoint(transform.position);
    }
}
