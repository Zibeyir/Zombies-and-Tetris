using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Modules")]
    [SerializeField] private WaveUI waveUI;
    [SerializeField] private HealthUI healthUI;
    [SerializeField] private CoinUI coinUI;
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private UIFader uiFader;
    [SerializeField] private ActiveBlocks activeBlocks;

    public WaveUI Wave => waveUI;
    public HealthUI Health => healthUI;
    public CoinUI Coin => coinUI;
    public GameOverUI GameOver => gameOverUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnWaveStarted += waveUI.ShowWave;
        GameEvents.OnFenceHPChanged += healthUI.UpdateHP;
        GameEvents.OnCoinChanged += coinUI.UpdateCoins;
        GameEvents.OnGameWon += gameOverUI.ShowWin;
        GameEvents.OnGameLost += gameOverUI.ShowLose;
        //GameEvents.UIFader += uiFader.FadeInAll(bool s);
    }

    private void OnDisable()
    {
        GameEvents.OnWaveStarted -= waveUI.ShowWave;
        GameEvents.OnFenceHPChanged -= healthUI.UpdateHP;
        GameEvents.OnCoinChanged -= coinUI.UpdateCoins;
        GameEvents.OnGameWon -= gameOverUI.ShowWin;
        GameEvents.OnGameLost -= gameOverUI.ShowLose;
    }

    // Optional wrapper methods
    public void SetWave(float wave, int waveNumber) => waveUI.ShowWave(wave,waveNumber);
    public void SetHP(float hp) => healthUI.UpdateHP(hp);
    public void SetCoins(int coins) => coinUI.UpdateCoins(coins);
    public void ShowWin() => gameOverUI.ShowWin();
    public void ShowLose() => gameOverUI.ShowLose();

    public void ActivatedBlockButton(bool ActiveBlockCase)=> uiFader.FadeINOutAll(ActiveBlockCase);
    public void ActivateButtonForSpawnBlocks(int coinValue) => activeBlocks.ActivetedBlockButtonforSpawn(coinValue);
    //public void FadeINOutAll(bool )
}
