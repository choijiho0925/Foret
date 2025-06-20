using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartController : MonoBehaviour
{
    public GameObject heartPrefab;
    public Transform heartContainer;
    public int initHeartsCount = 5;
    public int maxHeartsCount;

    private List<GameObject> heartList = new List<GameObject>();

    void Start()
    {
        InitHeart();
        UIManager.Instance.RegisterHeartController(this);
        int currentHeart = GameManager.Instance.GameData.playerHeart;

        for (int i = 0; i < maxHeartsCount - currentHeart; i++)
        {
            RemoveHeart();
        }
    }

    public void InitHeart()
    {
        heartList.Clear();

        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < initHeartsCount; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartContainer);
            heartList.Add(heart);
        }
        maxHeartsCount = heartList.Count;
    }

    public void RemoveHeart()
    {
        if (heartList.Count == 0) return;

        for (int i = heartList.Count - 1; i >= 0; i--)
        {
            GameObject heart = heartList[i];
            Transform fullHeart = heart.transform.Find("Full");

            if (fullHeart != null && fullHeart.gameObject.activeSelf)
            {
                fullHeart.gameObject.SetActive(false);
                break;
            }
        }
    }

    public void RecoverHeart()
    {
        for (int i = 0; i < heartList.Count; i++)
        {
            GameObject heart = heartList[i];
            Transform fullHeart = heart.transform.Find("Full");

            if (fullHeart != null && !fullHeart.gameObject.activeSelf)
            {
                fullHeart.gameObject.SetActive(true);
                break;
            }
        }
    }
}