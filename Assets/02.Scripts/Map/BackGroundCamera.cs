using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundCamera : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform backGround;
    [SerializeField] Transform player;

    private void Update()
    {
        if(player.position.x < 0)
        {
            transform.position = new Vector3(0, player.position.y + 4f, -10); // �÷��̾ ���ʿ� ���� �� ī�޶� ��ġ ���� 
        }
        else
        {
            transform.position = player.position + new Vector3(0, 4f, -10); // ī�޶�� �÷��̾� ��ġ�� ���� �̵�
        }
        backGround.position = transform.position + new Vector3(0, 1f, 9f); // ����� �÷��̾� ��ġ�� ���� �̵�
    }
}