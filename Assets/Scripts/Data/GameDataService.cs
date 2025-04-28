using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class GameDataService : MonoBehaviour
{
    public static GameDataService Instance { get; private set; }
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "ZombieGameData");
    public List<ActiveWeapon> activeWeapons;

    public ZombieGameData Data;

    public List<WeaponData> Weapons;
    public List<Material> WeaponMaterials;


    public Dictionary<string, WeaponData> weaponDict;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadData();
    }
    private void Start()
    {
        Weapons = Data.weapons;
        Initialize();
    }
    private void LoadData()
    {
        TextAsset json = Resources.Load<TextAsset>("GameData/ZombieGameData");
        if (json != null)
        {
            Data = JsonUtility.FromJson<ZombieGameData>(json.text);
            Debug.Log("ZombieGameData.json  found in Resources/GameData/");

        }
        else
        {
            Debug.LogError("ZombieGameData.json not found in Resources/GameData/");
        }
    }
    public EnemyData GetEnemyById(string id)
    {
        return Data.enemies.FirstOrDefault(e => e.Type == id);
    }

    public void Initialize()
    {
        weaponDict = new Dictionary<string, WeaponData>();

        foreach (var weapon in Weapons)
        {
            if (!weaponDict.ContainsKey(weapon.Type.ToString()))
            {
                weaponDict.Add(weapon.Type.ToString(), weapon);
            }
        }
    }
    public List<GameObject> GetActiveWeapons()
    {
        List<GameObject> activeWeaponObjects = new List<GameObject>();
        foreach (var activeWeapon in Data.weapons)
        {
            foreach(var weapon in activeWeapons)
            {
                if (activeWeapon.Type == weapon.Type.ToString()&& activeWeapon.UnlockCondition)
                {
                    activeWeaponObjects.Add(weapon.gameObject);
                    
                }
            }
        }
        return activeWeaponObjects;
    }
    public void UpdateWeaponData(WeaponData updatedWeapon)
    {
        if (updatedWeapon == null || Data == null || Data.weapons == null)
        {
            Debug.LogError("Data or updatedWeapon is null!");
            return;
        }

        // Type görə silib, yenisini əlavə edək
        for (int i = 0; i < Data.weapons.Count; i++)
        {
            if (Data.weapons[i].Type == updatedWeapon.Type)
            {
                Data.weapons[i] = updatedWeapon; // Əvəz et
                Debug.Log($"WeaponData updated for type: {updatedWeapon.Type}");
                SaveData(); // Dəyişən datanı saxlamaq
                return;
            }
        }

        // Əgər yoxdursa əlavə et (əgər istəsən)
        Debug.LogWarning($"WeaponData with type {updatedWeapon.Type} not found, adding new.");
        Data.weapons.Add(updatedWeapon);
        SaveData();
    }

    public void SaveData()
    {
        Debug.Log("Saving game data...");
        try
        {
            // JSON formatına serialize
            string json = JsonUtility.ToJson(Data, true);

            // Əgər folder yoxdursa yarat
            string folderPath = Path.GetDirectoryName(SaveFilePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // JSON faylı yaz
            File.WriteAllText(SaveFilePath, json);

            Debug.Log("Game data saved to: " + SaveFilePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to save game data: " + ex.Message);
        }
    }
    public WeaponData GetWeapon(WeaponType type)
    {
        if (weaponDict == null || weaponDict.Count == 0)
        {
            Initialize();
        }

        if (weaponDict.TryGetValue(type.ToString(), out var weapon))
        {
            return weapon;
        }

        Debug.LogWarning($"Weapon not found for type: {type}");
        return null;
    }

    // Access methods
    public List<WeaponData> GetWeaponData() => Data.weapons;
    public List<EnemyData> GetEnemyData() => Data.enemies;
    public List<WaveData> GetWaveData() => Data.waves;
    public List<GameStageData> GetStageData() => Data.gameStages;
    public List<UpgradeCostData> GetUpgradeCosts() => Data.upgradeCosts;
    public List<MapData> GetMapData() => Data.maps;
}

[System.Serializable]
public class ActiveWeapon
{
    public WeaponType Type;
    public GameObject gameObject;
}