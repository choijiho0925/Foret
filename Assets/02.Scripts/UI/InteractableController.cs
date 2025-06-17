using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public GameObject toInteractObject;
    public TextMeshProUGUI interactText;

    private void Start()
    {
        UIManager.Instance.RegisterInteractableController(this);
    }

    public void ShowInteractable(int layer)
    {
        toInteractObject.SetActive(true);
        if (layer == LayerMask.NameToLayer("SavePoint"))
        {
            interactText.text = "위치 기억하기";

        }
        else if (layer == LayerMask.NameToLayer("RuneStone"))
        {
            interactText.text = "돌을 만져보기";
        }
        else if(layer == LayerMask.NameToLayer("QuestItem"))
        {
            interactText.text = "줍기";
        }
        else
        {
            interactText.text = "대화하기";
        }
    }

    public void HideInteractable()
    {
        toInteractObject.SetActive(false);
    }
}