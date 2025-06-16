using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityController : MonoBehaviour
{
    public Image characterImage;

    private float fullAmount = 1f;
    private float currentAmount;

    private void Start()
    {
        currentAmount = fullAmount;
    }

    public void UseGauge(float amount)
    {
        characterImage.fillAmount = currentAmount - amount;
        currentAmount -= amount;
    }
}
