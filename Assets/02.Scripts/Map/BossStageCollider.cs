using UnityEngine;

public class BossStageCollider : MonoBehaviour
{
    [SerializeField] private NpcController npcController;
    
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            gameManager.NextIndex();
            npcController.GoNextPos();
        }
    }
}
