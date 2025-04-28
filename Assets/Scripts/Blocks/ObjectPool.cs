using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class BulletPoolItem
    {
        public BulletType Type;
        public GameObject Prefab;
        public int Size = 10;
    }

    [System.Serializable]
    public class EffectPoolItem
    {
        public EffectType Type;
        public GameObject Prefab;
        public int Size = 10;
    }
    public RectTransform CoinImage;
    public RectTransform CristalImage;

    public List<BulletPoolItem> BulletItems;
    public List<EffectPoolItem> EffectItems;

    private Dictionary<BulletType, Queue<GameObject>> bulletPoolDict;
    private Dictionary<BulletType, GameObject> bulletPrefabLookup;

    private Dictionary<EffectType, Queue<GameObject>> effectPoolDict;
    private Dictionary<EffectType, GameObject> effectPrefabLookup;

    private void Awake()
    {
        Instance = this;
        bulletPoolDict = new Dictionary<BulletType, Queue<GameObject>>();
        bulletPrefabLookup = new Dictionary<BulletType, GameObject>();

        effectPoolDict = new Dictionary<EffectType, Queue<GameObject>>();
        effectPrefabLookup = new Dictionary<EffectType, GameObject>();
    }

    private void Start()
    {
        foreach (var item in BulletItems)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < item.Size; i++)
            {
                GameObject obj = Instantiate(item.Prefab);
                obj.SetActive(false);
                obj.transform.SetParent(this.transform);
                objectQueue.Enqueue(obj);
            }

            bulletPoolDict[item.Type] = objectQueue;
            bulletPrefabLookup[item.Type] = item.Prefab;
        }

        foreach (var item in EffectItems)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < item.Size; i++)
            {
                GameObject obj = Instantiate(item.Prefab);
                obj.SetActive(false);
                obj.transform.SetParent(this.transform);
                objectQueue.Enqueue(obj);
            }

            effectPoolDict[item.Type] = objectQueue;
            effectPrefabLookup[item.Type] = item.Prefab;
        }
    }

    // BULLET spawn (damage ilə)
    public GameObject SpawnBullet(BulletType type, Vector3 position, Quaternion rotation, int damage = 0)
    {
       // Debug.Log($"Spawning bullet of type: {type} at position: {position} with rotation: {rotation} and damage: {damage}");
        if (!bulletPoolDict.ContainsKey(type))
        {
            Debug.LogWarning($"No bullet pool found for type: {type}");
            return null;
        }

        GameObject obj = GetInactiveFromPool(bulletPoolDict[type]);

        if (obj == null)
        {
            obj = Instantiate(bulletPrefabLookup[type], position, rotation);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);
            bulletPoolDict[type].Enqueue(obj);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        if (damage != 0)
            obj.GetComponent<BulletBase>().Initiliaze(damage);

        obj.SetActive(true);
        return obj;
    }
    public GameObject SpawnCoin(EffectType type, Vector3 position, Quaternion rotation)
    {
        // Debug.Log($"Spawning bullet of type: {type} at position: {position} with rotation: {rotation} and damage: {damage}");
       

        GameObject obj = GetInactiveFromPool(effectPoolDict[type]);

        if (obj == null)
        {
            obj = Instantiate(effectPrefabLookup[type], position, rotation);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);
            effectPoolDict[type].Enqueue(obj);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        if (type == EffectType.Coin)
        {
            obj.GetComponent<CoinMovement>().FlyToUI(CoinImage, position);

        }
        else if (type == EffectType.Cristal)
        {
            obj.GetComponent<CoinMovement>().FlyToUI(CristalImage, position);

        }

        obj.SetActive(true);
        return obj;
    }
    // EFFECT spawn (damage-siz)
    public GameObject SpawnEffect(EffectType type, Vector3 position, Quaternion rotation)
    {
        if (!effectPoolDict.ContainsKey(type))
        {
            Debug.LogWarning($"No effect pool found for type: {type}");
            return null;
        }

        GameObject obj = GetInactiveFromPool(effectPoolDict[type]);

        if (obj == null)
        {
            obj = Instantiate(effectPrefabLookup[type], position, rotation);
            obj.SetActive(false);
            obj.transform.SetParent(this.transform);
            effectPoolDict[type].Enqueue(obj);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        return obj;
    }

    private GameObject GetInactiveFromPool(Queue<GameObject> pool)
    {
        foreach (var pooledObj in pool)
        {
            if (!pooledObj.activeInHierarchy)
            {
                return pooledObj;
            }
        }
        return null;
    }
}
public enum BulletType
{
    Ak47,
    Pistol,
    Grenade,
    Firegun,
    Shotgun,
    Snaper
}

public enum EffectType
{
    ZombieBlood,
    ShotgunTouchExplode,
    ShotgunTouchExplodeFire,
    GrenadeExplode,
    Coin,
    Cristal
}

public enum Money
{
    Coin,Cristal
}