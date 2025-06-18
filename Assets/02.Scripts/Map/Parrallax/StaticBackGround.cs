using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBackGround : MonoBehaviour
{
    [SerializeField] GameObject backGround;
    [SerializeField] Transform transform;

    private void Update()
    {
        backGround.transform.position = new Vector3(transform.position.x, transform.position.y, backGround.transform.position.z);
    }
}
