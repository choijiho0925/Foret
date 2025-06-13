using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    private float delayTime = 0.5f; // �÷��̾ �ٸ� ���� ���� �� ���� �ð�
    private float respawnTime = 5f; // �ٸ��� ������ �� ����� �ð�
    private bool hasFallen; // �ٸ��� ���������� ���θ� Ȯ���ϴ� ����
    private Vector3 startPosition; // �ٸ��� �ʱ� ��ġ
    private Rigidbody2D rb; // Rigidbody2D ������Ʈ

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position; // �ٸ��� �ʱ� ��ġ ����
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !hasFallen)
        {
            StartCoroutine(Fall()); // �÷��̾ �ٸ� ���� ���� �� �ٸ��� ���������� �ڷ�ƾ ����
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeadZone"))
        {
            StartCoroutine(Respawn()); // �������� ������ Respawn �޼��� ȣ��
        }
    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(delayTime);

        rb.bodyType = RigidbodyType2D.Dynamic; // �ٸ��� ���� Rigidbody�� ����
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime); // ������ �ð� ���� ���

        ResetSetting(); // �ٸ��� ������ �ʱ�ȭ�ϴ� �޼��� ȣ��

    }

    private void ResetSetting()
    {
        rb.bodyType = RigidbodyType2D.Static; // �ٸ��� ���� Rigidbody�� ����
        transform.position = startPosition; // �ٸ��� ��ġ�� �ʱ� ��ġ�� �ǵ���
    }
}
