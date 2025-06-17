using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundInTitleScene : MonoBehaviour
{
    public float scrollSpeed = 1f;
    private Vector3 startPosition;
    private float spriteWidth;

    void Start()
    {
        startPosition = transform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            spriteWidth = sr.bounds.size.x;
        }
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, spriteWidth);
        transform.position = startPosition + Vector3.left * newPosition;
    }
}
