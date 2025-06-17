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
        initHeartsCount = GameManager.Instance.GameData.playerHeart;
        InitHeart();
        UIManager.Instance.RegisterHeartController(this);
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