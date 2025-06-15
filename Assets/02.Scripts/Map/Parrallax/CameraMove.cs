using UnityEngine;

public class CameraMove : MonoBehaviour
{
	[SerializeField][Range(-1.0f, 1.0f)]
	private	float	moveSpeed = 1;

	private void Update()
	{
		transform.position += Vector3.right * moveSpeed * Time.deltaTime;
	}
}

