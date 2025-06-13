using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InstantObject : MonoBehaviour
{
    [SerializeField] private float duration;

    private void OnEnable()
    {
        Destroy(gameObject, duration);
    }
}
