using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool<TEnum, TMono> : MonoBehaviour
    where TEnum : System.Enum
    where TMono : MonoBehaviour
{
    // 인스펙터 설정을 위한 내부 클래스도 제네릭으로 변경
    [System.Serializable]
    public class Pool
    {
        public TEnum type;
        public TMono prefab;
        public int size;    //풀 초기화 사이즈
    }

    public List<Pool> pools;
    private Dictionary<TEnum, Queue<TMono>> poolDictionary;

    protected virtual void Awake()
    {
        poolDictionary = new Dictionary<TEnum, Queue<TMono>>();

        foreach (Pool pool in pools)
        {
            Queue<TMono> objectQueue = new Queue<TMono>();
            for (int i = 0; i < pool.size; i++)
            {
                TMono obj = Instantiate(pool.prefab, transform);
                obj.gameObject.SetActive(false);
                objectQueue.Enqueue(obj);
            }
            poolDictionary.Add(pool.type, objectQueue);
        }
    }

    // 외부에서 오브젝트를 요청할 때 사용하는 함수 
    public TMono Get(TEnum type, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(type)) return null;

        Queue<TMono> poolQueue = poolDictionary[type];
        TMono objToGet;

        if (poolQueue.Count > 0)
        {
            objToGet = poolQueue.Dequeue();
        }
        else
        {
            Pool pool = pools.Find(p => p.type.Equals(type));
            objToGet = Instantiate(pool.prefab, transform);
        }

        objToGet.transform.position = position;
        objToGet.transform.rotation = rotation;
        objToGet.gameObject.SetActive(true);
        return objToGet;
    }

    // 외부에서 오브젝트를 요청할 때 사용하는 함수 
    public void Return(TEnum type, TMono obj)
    {
        if (!poolDictionary.ContainsKey(type)) return;

        obj.gameObject.SetActive(false);
        poolDictionary[type].Enqueue(obj);
    }
}