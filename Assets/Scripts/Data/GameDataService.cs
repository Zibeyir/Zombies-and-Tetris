using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class GameDataService : MonoBehaviour
{
    public static GameDataService Instance { get; private set; }

    private string SaveFilePath;
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

        SaveFilePath = Path.Combine(Application.persistentDataPath, "GameData/ZombieGameData.json");
        LoadData();
    }

    private void Start()
    {
        Weapons = Data.weapons;
        Initialize();
    }

    private void LoadData()
    {
        if (File.Exists(SaveFilePath))
        {
            // Load from persistent data path
            string json = File.ReadAllText(SaveFilePath);
            Data = JsonUtility.FromJson<ZombieGameData>(json);
            Debug.Log("Game data loaded from persistent path.");
        }
        else
        {
            // First time: copy from Resources to persistent path
            TextAsset jsonAsset = Resources.Load<TextAsset>("GameData/ZombieGameData");
            if (jsonAsset != null)
            {
                Data = JsonUtility.FromJson<ZombieGameData>(jsonAsset.text);

                // Save initial copy for editing at runtime
                string folder = Path.GetDirectoryName(SaveFilePath);
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                File.WriteAllText(SaveFilePath, jsonAsset.text);

                Debug.Log("ZombieGameData.json copied from Resources to persistent path.");
            }
            else
            {
                Debug.LogError("ZombieGameData.json not found in Resources/GameData/");
            }
        }
    }

    public void Initialize()
    {
        weaponDict = new Dictionary<string, WeaponData>();
        foreach (var weapon in Weapons)
        {
            if (!weaponDict.ContainsKey(weapon.Type.ToString()))
                weaponDict.Add(weapon.Type.ToString(), weapon);
        }
    }

    public EnemyData GetEnemyById(string id)
    {
        return Data.enemies.FirstOrDefault(e => e.Type == id);
    }

    public List<GameObject> GetActiveWeapons()
    {
        List<GameObject> activeWeaponObjects = new List<GameObject>();
        foreach (var activeWeapon in Data.weapons)
        {
            foreach (var weapon in activeWeapons)
            {
                if (activeWeapon.Type == weapon.Type.ToString() && activeWeapon.UnlockCondition)
                    activeWeaponObjects.Add(weapon.gameObject);
            }
        }
        return activeWeaponObjects;
    }

    public void UpdateWeaponData(WeaponData updatedWeapon)
    {
        if (updatedWeapon == null || Data?.weapons == null)
        {
            Debug.LogError("Data or updatedWeapon is null!");
            return;
        }

        for (int i = 0; i < Data.weapons.Count; i++)
        {
            if (Data.weapons[i].Type == updatedWeapon.Type)
            {
                Data.weapons[i] = updatedWeapon;
                Debug.Log($"WeaponData updated for type: {updatedWeapon.Type}");
                SaveData();
                return;
            }
        }

        Debug.LogWarning($"WeaponData with type {updatedWeapon.Type} not found, adding new.");
        Data.weapons.Add(updatedWeapon);
        SaveData();
    }

    public void SaveData()
    {
        try
        {
            string json = JsonUtility.ToJson(Data, true);
            string folder = Path.GetDirectoryName(SaveFilePath);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
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
            Initialize();

        if (weaponDict.TryGetValue(type.ToString(), out var weapon))
            return weapon;

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
