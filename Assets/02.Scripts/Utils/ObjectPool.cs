using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    private T prefab; // 풀링할 오브젝트의 프리팹 (T 타입의 컴포넌트)

    [SerializeField]
    private int poolSize = 10; // 초기 풀 사이즈

    // 실제 오브젝트들을 보관할 큐(Queue)
    private Queue<T> pool = new Queue<T>();

    protected virtual void Awake()
    {
        InitializePool();
    }

    // 풀을 초기화하는 함수
    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            T obj = CreateNewObject();
            pool.Enqueue(obj);
        }
    }

    // 새로운 오브젝트를 생성하고 비활성화 상태로 만드는 함수
    private T CreateNewObject()
    {
        T newObj = Instantiate(prefab, transform); // 자식으로 생성
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    // 외부에서 오브젝트를 요청할 때 사용하는 함수 
    public T Get(Vector3 position, Quaternion rotation)
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.gameObject.SetActive(true);
            return obj;
        }
        else // 풀이 비어있으면 새로 생성
        {
            T newObj = CreateNewObject();
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    // 외부에서 사용이 끝난 오브젝트를 반납할 때 사용하는 함수
    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}