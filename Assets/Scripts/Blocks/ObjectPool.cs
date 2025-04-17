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
        public int Size = 10;
    }

    public List<PoolItem> Items;
    private Dictionary<BulletType, Queue<GameObject>> poolDict;
    private Dictionary<BulletType, GameObject> prefabLookup;

    private void Awake()
    {
        Instance = this;
        poolDict = new Dictionary<BulletType, Queue<GameObject>>();
        prefabLookup = new Dictionary<BulletType, GameObject>();

        
    }
    private void Start()
    {
        foreach (var item in Items)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < item.Size; i++)
            {
                GameObject obj = Instantiate(item.Prefab);
                obj.SetActive(false);
                obj.transform.SetParent(this.transform);
                objectQueue.Enqueue(obj);
            }

            poolDict[item.Type] = objectQueue;
            prefabLookup[item.Type] = item.Prefab;
        }
    }
    public GameObject SpawnFromPool(BulletType type, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(type))
        {
            Debug.LogWarning($"No pool found for bullet type: {type}");
            return null;
        }

        GameObject obj = null;

        // Birini tapana qədər baxırıq
        foreach (var pooledObj in poolDict[type])
        {
            if (!pooledObj.activeInHierarchy)
            {
                obj = pooledObj;
                break;
            }
        }

        // Əgər boş yoxdursa — yenisini yarat
        if (obj == null)
        {
            obj = Instantiate(prefabLookup[type], position,rotation);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);
            poolDict[type].Enqueue(obj); 
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }
}
