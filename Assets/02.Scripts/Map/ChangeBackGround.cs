using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBackGround : MonoBehaviour
{
    [SerializeField] GameObject firstBackGroundObj;
    [SerializeField] GameObject secondBackGroundObj;
    [SerializeField] GameObject fadeOut;
    [SerializeField] private NpcController npcController;

    private float fadeDuration = 3f; // 페이드 아웃/인 시간
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager.CanGoNextStage)
        {
            secondBackGroundObj.SetActive(true);
            firstBackGroundObj.SetActive(false);
        }
        else
        {
            secondBackGroundObj.SetActive(false);
            firstBackGroundObj.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(ChangeBackgroundCoroutine());
        }
    }

    private IEnumerator ChangeBackgroundCoroutine()
    {
        firstBackGroundObj.SetActive(false);

        fadeOut.SetActive(true);

        Image fadeImage = fadeOut.GetComponentInChildren<Image>();

        Color fadeColor = fadeImage.color;
        fadeColor.a = 1f; // 초기 투명도 설정
        fadeImage.color = fadeColor;

        // 알파를 0으로 페이드 아웃
        yield return fadeImage.DOFade(0f, fadeDuration).WaitForCompletion();

        secondBackGroundObj.SetActive(true);
    }
}