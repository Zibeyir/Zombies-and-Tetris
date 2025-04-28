using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public SaveData _SaveData { get; private set; }
    [NonSerialized]public int BlockPrice;
    [SerializeField] public WeaponUpgrade UpgradeUI;
    int currentLevel;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        BlockPrice = 50;
        Instance = this;
        _SaveData = SaveDataService.Load();
    }

    private void Start()
    {
        //LevelComplete();
        //SaveDataService.DeleteSave();
        currentLevel = SaveDataService.Load().CurrentLevel;
        Debug.Log("Current Level: " + currentLevel);

        if (currentLevel == 6)
        {
            SaveDataService.DeleteSave();
            currentLevel = 5;
        }
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int levelIndex)
    {
        var map = GameDataService.Instance.GetMapData()[levelIndex];
         // You may filter waves per map

        GameObject fence = GameObject.FindGameObjectWithTag("Fence");
        fence.GetComponent<Fence>().GetHP(map.HPStart);

        StartWaveForLevel();
    }

    public void LevelComplete()
    {
        GameEvents.OnGameWon?.Invoke();
        UIManager.Instance.ShowWin();

        SaveData data = SaveDataService.Load();
        data.CurrentLevel++;
        data.Coins = _SaveData.Coins;
        data.Cristal = _SaveData.Cristal+50;
        SaveDataService.Save(data);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(0);
    }
    public void NextLevelDefeat()
    {
        SceneManager.LoadScene(0);
    }
    void SaveFinalData()
    {

    }

    public void StartWaveForLevel()
    {
        var waves = GameDataService.Instance.GetWaveData();
        //UpgradeUI.SetActive(false);
        

        WaveManager.Instance.StartWaves(waves);
        if (currentLevel == 0)
        {

            WaveManager.Instance.StartWavesAfterUpgrade();
        }
        else
        {
            UpgradeUI.OpenWeaponUpgradePanel(GameDataService.Instance.GetWeaponData());

            //UpgradeUI.SetActive(true);
            //WaveManager.Instance.StartWaves(waves);
        }
    }
    public void GameOver()
    {
        SaveData data = SaveDataService.Load();
        data.Cristal = _SaveData.Cristal;

        SaveDataService.Save(data);
        UIManager.Instance.ShowLose();
    }
}
