using UnityEngine;

public class SaveSpawnPoint : MonoBehaviour, IInteractable
{
    private bool playerInZone; // 플레이어가 영역에 있는지 여부를 나타내는 변수
    private SpriteRenderer renderer; // 스프라이트 렌더러 컴포넌트
    public bool PlayerInZone => playerInZone; // 외부에서 플레이어가 영역에 있는지 확인할 수 있도록 프로퍼티로 노출
    public Material normalMaterial; // 일반 재질
    public Material outMaterial; // 플레이어가 영역에 있을 때 적용할 재질
    private PlayerInteract player;

    private void Start()
    {
        renderer = GetComponentInChildren<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 초기화
        player = FindAnyObjectByType<PlayerInteract>();
    }

    public void ShowInteractUI()
    {
        //상호작용 UI 표시
    }

    public void InteractAction()
    {
        GameManager.Instance.SetRespawnPoint(this.transform.position);
        player.OnEndInteraction();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = true; // 플레이어가 영역에 들어왔음을 표시
            renderer.material = outMaterial; // 플레이어가 영역에 들어오면 outMaterial로 재질 변경
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerInZone = false; // 플레이어가 영역을 벗어났음을 표시
            renderer.material = normalMaterial; // 플레이어가 영역을 벗어나면 normalMaterial로 재질 변경
        }
    }


}