using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    [System.Serializable]
    public struct Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        Instance = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log(tag);
            return null;
        }
            

        GameObject obj = poolDictionary[tag].Dequeue();
        poolDictionary[tag].Enqueue(obj);
        Debug.Log(tag);

        return obj;
    }
}
