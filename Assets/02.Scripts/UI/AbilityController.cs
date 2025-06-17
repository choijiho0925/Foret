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
        UpdateGauge(GameManager.Instance.GameData.playerEnergy / 100f);
        UIManager.Instance.RegisterAbilityController(this);
    }

    public void UpdateGauge(float amount)
    {
        characterImage.fillAmount = amount;
        currentAmount = amount;
        currentAmount = Mathf.Clamp(currentAmount, 0, fullAmount);
    }
}
