using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public GameObject toInteractObject;
    public TextMeshProUGUI interactText;

    public void ShowInteractable(int layer)
    {
        toInteractObject.SetActive(true);
        if (layer == LayerMask.NameToLayer("SavePoint"))
        {
            interactText.text = "위치 기억하기";
            
        }

        interactText.text = "대화하기";
    }

    public void HideInteractable()
    {
        toInteractObject.SetActive(false);
    }
}