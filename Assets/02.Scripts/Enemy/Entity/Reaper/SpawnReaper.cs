using System.Collections;
using UnityEngine;

public class SpawnReaper : MonoBehaviour
{
    [SerializeField] private GameObject reaperPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnTime = 3f;
    [SerializeField] private bool isSpawn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSpawn && collision.CompareTag("Player"))
        {
            Debug.Log("리퍼 소환");
            StartCoroutine(SpawningReaper());
            isSpawn = true;
        }
    }

    private IEnumerator SpawningReaper()
    {
        yield return new WaitForSeconds(spawnTime);

        Instantiate(reaperPrefab, spawnPoint.position, Quaternion.identity);
    }
}
