using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneStone : MonoBehaviour, IInteractable
{
    private bool canGoNextStage; // 다음 스테이지로 넘어갈 수 있는지 여부
    private bool isPlayerInZone; // 플레이어가 영역에 있는지 여부
    private Renderer renderer;
    public Material normalMaterial;
    public Material outLineMaterial;
    public GameObject interactGameObject; // 상호작용 오브젝트

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void ShowInteractUI()
    {
        
    }

    public void InteractAction()
    {
        if (canGoNextStage && isPlayerInZone)
        {
            OpenNextStage(); // 다음 스테이지로 넘어가는 메소드 호출
        }
    }

    private void OpenNextStage()
    {
        interactGameObject.SetActive(false); // 다음 스테이지로 넘어갈 수 있도록 상호작용 오브젝트 비활성화
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerInZone = true; // 플레이어가 영역에 들어옴
            renderer.material = outLineMaterial; // 플레이어가 영역에 들어오면 아웃라인 머티리얼로 변경
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerInZone = false; // 플레이어가 영역을 벗어남
            renderer.material = normalMaterial; // 플레이어가 영역을 벗어나면 원래 머티리얼로 변경
        }
    }
}
