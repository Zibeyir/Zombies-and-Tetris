using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        int currentLevel = SaveDataService.Load().CurrentLevel;
        LoadLevel(currentLevel);
    }

    public void LoadLevel(int levelIndex)
    {
        var map = GameDataService.Instance.GetMapData()[levelIndex];
        var waves = GameDataService.Instance.GetWaveData(); // You may filter waves per map

        GameObject fence = GameObject.FindGameObjectWithTag("Fence");
        fence.GetComponent<Fence>().HP = map.HPStart;

        WaveManager.Instance.StartWaves(waves);
    }

    public void LevelComplete()
    {
        GameEvents.OnGameWon?.Invoke();
        UIManager.Instance.ShowWinScreen();

        SaveData data = SaveDataService.Load();
        data.CurrentLevel++;
        SaveDataService.Save(data);
    }


    public void GameOver()
    {
        //UIManager.Instance.ShowLoseScreen();
    }
}
