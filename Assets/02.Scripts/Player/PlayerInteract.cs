using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("상호작용 가능 레이어")]
    [SerializeField] private LayerMask interactableLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((interactableLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("Player Interact + " + collision.gameObject.name);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // 키가 눌렸고, 상호작용할 대상이 있을 때
        // if (context.phase == InputActionPhase.Started && currentInteractable != null)
        // {
        //     // PlayerCtrl을 통해 플레이어의 움직임을 막고,
        //     // 대상 오브젝트의 상호작용을 시작함
        //     currentInteractable.StartInteraction(playerCtrl);
        // }
    }
}
