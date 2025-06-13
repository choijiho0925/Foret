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
            transform.position = new Vector3(0, player.position.y + 4f, -10); // 플레이어가 왼쪽에 있을 때 카메라 위치 조정 
        }
        else
        {
            transform.position = player.position + new Vector3(0, 4f, -10); // 카메라는 플레이어 위치에 따라 이동
        }
        backGround.position = transform.position + new Vector3(0, 1f, 9f); // 배경은 플레이어 위치에 따라 이동
    }
}