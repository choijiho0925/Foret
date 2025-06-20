using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;      // 카메라의 Transform 컴포넌트

    private Vector3 cameraStartPosition;    // 게임을 시작했을 때 카메라의 시작 위치
    private float distance;             // cameraStartPosition으로 부터 현재 카메라까지의 x 이동거리

    private Material[] materials;               // 배경 스크롤을 위한 Material 배열 변수
    private float[] layerMoveSpeed;         // z 값이 다른 배경 레이어 별 이동 속도

    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float parallaxSpeed;            // layerMoveSpeed에 곱해서 사용하는 배경 스크롤 이동 속도

    private void Awake()
    {
        // 게임을 시작할 때 카메라의 위치 저장 (이동 거리 계산용)
        cameraStartPosition = cameraTransform.position;

        // 배경의 개수를 구하고, 배경 정보를 저장할 GameObject 배열 선언
        int backgroundCount = transform.childCount;
        GameObject[] backgrounds = new GameObject[backgroundCount];

        // 각 배경의 material과 이동 속도를 저장할 배열 선언
        materials = new Material[backgroundCount];
        layerMoveSpeed = new float[backgroundCount];

        // GetChild() 메소드를 호출해 자식으로 있는 배경 정보들을 불러온다
        for (int i = 0; i < backgroundCount; ++i)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            materials[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        // 레이어(카메라와의 z 거리 기준)별로 이동 속도 설정
        CalculateMoveSpeedByLayer(backgrounds, backgroundCount);
    }

    private void LateUpdate()
    {
        // 카메라가 이동한 거리 = 카메라의 현재 위치 - 시작 위치
        distance = cameraTransform.position.x - cameraStartPosition.x;
        // 배경의 x 위치를 현재 카메라의 x 위치로 설정
        if (cameraTransform.position.x < 0)
        {
            transform.position = new Vector3(0, cameraTransform.position.y, 0);
        }
        else
        {
            transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, 0);
        }

        // 레이어별로 현재 배경이 출력되는 offset 설정
        for (int i = 0; i < materials.Length; ++i)
        {
            float speed = layerMoveSpeed[i] * parallaxSpeed;
            materials[i].SetTextureOffset("_MainTex", new Vector2(distance, 0) * speed);
        }
    }

    private void CalculateMoveSpeedByLayer(GameObject[] backgrounds, int count)
    {
        float closeDistance = float.MaxValue;
        float farDistance = float.MinValue;

        // 모든 배경의 카메라로부터의 z 거리 측정
        for (int i = 0; i < count; ++i)
        {
            float zDistance = Mathf.Abs(backgrounds[i].transform.position.z - cameraTransform.position.z);
            if (zDistance < closeDistance) closeDistance = zDistance;
            if (zDistance > farDistance) farDistance = zDistance;
        }

        // 안전 처리: 거리가 모두 같으면 동일 속도 부여
        float distanceRange = Mathf.Max(farDistance - closeDistance, 0.0001f);

        for (int i = 0; i < count; ++i)
        {
            float zDistance = Mathf.Abs(backgrounds[i].transform.position.z - cameraTransform.position.z);

            // 가까운 배경은 1에 가까운 값, 멀수록 0에 가까운 값
            float t = 1f - ((zDistance - closeDistance) / distanceRange);
            layerMoveSpeed[i] = Mathf.Lerp(0.1f, 1f, t);  // 최소 속도 0.1 ~ 최대 속도 1
        }
    }
}