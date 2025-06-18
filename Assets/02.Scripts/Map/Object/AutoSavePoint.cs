using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSavePoint : MonoBehaviour
{
    private bool canAutoSave = true;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && canAutoSave)
        {
            GameManager.Instance.SetRespawnPoint(collision.transform.position);
            Debug.Log(GameManager.Instance.respawnPoint);
            canAutoSave = false;
        }
    }
}