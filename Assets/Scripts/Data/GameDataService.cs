using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameDataService : MonoBehaviour
{
    public static GameDataService Instance { get; private set; }

    public ZombieGameData Data;

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

    // Access methods
    public List<WeaponData> GetWeaponData() => Data.weapons;
    public List<EnemyData> GetEnemyData() => Data.enemies;
    public List<WaveData> GetWaveData() => Data.waves;
    public List<GameStageData> GetStageData() => Data.gameStages;
    public List<UpgradeCostData> GetUpgradeCosts() => Data.upgradeCosts;
    public List<MapData> GetMapData() => Data.maps;
}

