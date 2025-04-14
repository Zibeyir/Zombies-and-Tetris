using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Text WaveText;
    public Slider FenceHPSlider;
    public GameObject WinScreen;
    public GameObject LoseScreen;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameEvents.OnWaveStarted += ShowWave;
        GameEvents.OnFenceHPChanged += UpdateFenceHP;
        GameEvents.OnGameWon += ShowWinScreen;
        GameEvents.OnGameLost += ShowLoseScreen;
    }

    private void OnDisable()
    {
        GameEvents.OnWaveStarted -= ShowWave;
        GameEvents.OnFenceHPChanged -= UpdateFenceHP;
        GameEvents.OnGameWon -= ShowWinScreen;
        GameEvents.OnGameLost -= ShowLoseScreen;
    }

    public void ShowWave(int waveNumber)
    {
        WaveText.text = "Wave " + waveNumber;
    }

    public void UpdateFenceHP(int hp)
    {
        FenceHPSlider.value = hp;
    }

    public void ShowWinScreen()
    {
        WinScreen.SetActive(true);
    }

    public void ShowLoseScreen()
    {
        LoseScreen.SetActive(true);
    }
}
