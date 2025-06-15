using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.TryGetComponent<PlayerStat>(out var playerStat))
            {
                playerStat.DamageAndRespawn(1);
            }
        }
    }
}