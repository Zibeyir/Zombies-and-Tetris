using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class PoolItem
    {
        public BulletType Type;
        public GameObject Prefab;
        public float Size = 10;
    }

    public List<PoolItem> Items;
    private Dictionary<BulletType, Queue<GameObject>> poolDict;

    private void Awake()
    {
        Instance = this;

        poolDict = new Dictionary<BulletType, Queue<GameObject>>();

        foreach (var item in Items)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            for (int i = 0; i < item.Size; i++)
            {
                GameObject obj = Instantiate(item.Prefab);
                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }
            poolDict[item.Type] = objectQueue;
        }
    }

    public GameObject SpawnFromPool(BulletType type, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(type)) return null;

        GameObject obj = poolDict[type].Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        poolDict[type].Enqueue(obj);
        return obj;
    }
}
