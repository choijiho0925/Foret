using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] GameObject platform; // �÷��� ������Ʈ

    private float moveDistance = 3f; // �÷��� �̵��Ÿ�
    private float moveSpeed = 2f; // �÷��� �̵��ӵ�
    private float disappearTime = 1f; // �÷��� ������� �ð�
    private float respawnTime = 1.5f; // �÷��� ����� �ð�
    private bool movingToTarget = true; // �÷����� ��ǥ ��ġ�� �̵� ������ ����
    private Vector3 startPosition; // �÷��� ���� ��ġ
    private Vector3 targetPosition; // �÷��� ��ǥ ��ġ
    private Transform player; // �÷��̾� Ʈ������


    private void Start()
    {
        CheckDirection();
    }

    private void Update()
    {
        MovePlatform(); 
    }

    private void CheckDirection() // �÷��� �̵� ������ �����ϴ� �޼���
    {
        startPosition = platform.transform.position; // �÷����� ���� ��ġ ����

        bool isMoveDirection = Random.Range(0, 2) > 0; // �̵� ���� ���� (0 �Ǵ� 1)
        float direction = isMoveDirection ? 1f : -1f; // 1�̸� ������, -1�̸� �������� �̵�
        targetPosition = startPosition + new Vector3(moveDistance * direction, 0, 0); // ��ǥ ��ġ ����
    }

    private void MovePlatform() // �÷��� �̵� �޼��� ȣ��
    {
        Vector3 arrivalPoint = movingToTarget ? targetPosition : startPosition; // ���� ��ǥ ��ġ ����

        float fixedY = startPosition.y; // �÷����� Y ��ǥ�� �����ϱ� ���� ����
        Vector3 currentPosition = platform.transform.position; // ���� �÷��� ��ġ
        Vector3 targetFixedY = new Vector3(arrivalPoint.x, fixedY, arrivalPoint.z); // ��ǥ ��ġ�� Y ��ǥ�� ����

        platform.transform.position = Vector3.MoveTowards(currentPosition, targetFixedY, moveSpeed * Time.deltaTime); // �÷��� �̵�

        if (Vector3.Distance(platform.transform.position, arrivalPoint) < 0.01f) // ��ǥ ��ġ�� �����ߴ��� Ȯ��
        {
            movingToTarget = !movingToTarget; // �̵� ���� ��ȯ
        }
    }

    private void ClearParent() // �÷��̾ �÷����� �ڽĿ��� �����ϴ� �޼���
    {
        if (player != null && player.parent == transform) // �÷��̾ �÷����� �ڽ����� �����Ǿ� �ִ��� Ȯ��
        {
            player.SetParent(null); // �÷��̾ �÷����� �ڽĿ��� ����
            player = null; // �÷��̾� Ʈ������ �ʱ�ȭ
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) // �÷��̾� ���̾� Ȯ��
        {
            player = collision.transform; // �÷��̾� Ʈ������ ����
            StartCoroutine(SetPlayerAsChildNextFrame());
            StartCoroutine(Respawn()); // �÷��̾ �÷����� ������ ����� �ڷ�ƾ ����
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) // �÷��̾ �÷������� ����� ��
        {
            ClearParent(); // �÷��̾ �÷����� �ڽĿ��� ����
        }
    }

    private IEnumerator SetPlayerAsChildNextFrame()
    {
        yield return null; // �� ������ ���
        player.SetParent(transform);
    }

    private IEnumerator Respawn() // �÷��� ����� �ڷ�ƾ
    {
        yield return new WaitForSeconds(disappearTime); // �÷��� ������� �ð� ���� ���

        ClearParent(); // �÷��̾ �÷����� �ڽĿ��� ����

        platform.GetComponent<Renderer>().enabled = false; // �÷��� ������ ��Ȱ��ȭ
        platform.GetComponent<Collider2D>().enabled = false; // �÷��� �浹ü ��Ȱ��ȭ

        platform.transform.position = startPosition; // �÷��� ��ġ�� ���� ��ġ�� �ǵ���

        yield return new WaitForSeconds(respawnTime); // ������ �ð� ���� ���

        platform.GetComponent<Renderer>().enabled = true; // �÷��� ������ Ȱ��ȭ
        platform.GetComponent<Collider2D>().enabled = true; // �÷��� �浹ü Ȱ��ȭ
    }
}