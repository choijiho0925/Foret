using _02.Scripts.Player;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("상호작용 가능 레이어")]
    [SerializeField] private LayerMask interactableLayer;

    [Header("적 레이어")]
    [SerializeField] private LayerMask enemyLayer;

    private PlayerCtrl playerCtrl;
    private PlayerMovement playerMovement;
    private PlayerStat playerStat;
    private IInteractable currentInteractable;

    private void Awake()
    {
        playerCtrl = GetComponent<PlayerCtrl>();
        playerMovement = GetComponent<PlayerMovement>();
        playerStat = GetComponent<PlayerStat>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((interactableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent<IInteractable>(out currentInteractable))
            {
                currentInteractable.ShowInteractUI();
            }
        }
        else if ((enemyLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            playerStat.TakeDamage(1);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentInteractable == null) return;

        if ((interactableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            currentInteractable = null;
            UIManager.Instance.interactableController.HideInteractable();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //키가 눌렸고, 상호작용할 대상이 있을 때
        if (context.phase == InputActionPhase.Started && currentInteractable != null)
        {
            // PlayerCtrl을 통해 플레이어의 움직임을 막고,
            // 대상 오브젝트의 상호작용을 시작함
            playerCtrl.EnterInteraction();
            playerMovement.Stop();
            currentInteractable.InteractAction();
        }
    }

    public void OnEndInteraction()
    {
        playerCtrl.ExitInteraction();
    }
}
