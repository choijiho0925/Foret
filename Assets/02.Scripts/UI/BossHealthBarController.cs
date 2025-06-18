using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarController : MonoBehaviour
{
    public Slider healthBar;

    private void Start()
    {
        healthBar.value = 1.0f;
        UIManager.Instance.RegisterBossHealthBar(this);
    }
}
