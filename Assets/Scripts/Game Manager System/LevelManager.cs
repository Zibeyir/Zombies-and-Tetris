using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public SaveData _SaveData { get; private set; }
    public int BlockPrice = 50;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _SaveData = SaveDataService.Load();
    }

    private void Start()
    {
        //SaveDataService.DeleteSave();
        int currentLevel = SaveDataService.Load().CurrentLevel;
        if (currentLevel == 5)
        {
            SaveDataService.DeleteSave();
            currentLevel = 0;
        }
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int levelIndex)
    {
        var map = GameDataService.Instance.GetMapData()[levelIndex];
        var waves = GameDataService.Instance.GetWaveData(); // You may filter waves per map

        GameObject fence = GameObject.FindGameObjectWithTag("Fence");
        fence.GetComponent<Fence>().GetHP(map.HPStart);

        WaveManager.Instance.StartWaves(waves);
    }

    public void LevelComplete()
    {
        GameEvents.OnGameWon?.Invoke();
        UIManager.Instance.ShowWin();

        SaveData data = SaveDataService.Load();
        data.CurrentLevel++;
        data.Coins = _SaveData.Coins;
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
    public void GameOver()
    {
        SaveData data = SaveDataService.Load();
        data.Coins = _SaveData.Coins;

        SaveDataService.Save(data);
        UIManager.Instance.ShowLose();
    }
}
