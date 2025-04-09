using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    [SerializeField] private UIManager uiManager;

    private int totalCoins = 0;
    private bool isGameActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (uiManager != null)
        {
            uiManager.onGameStart.AddListener(StartGame);
            uiManager.onGameRestart.AddListener(RestartGame);
        }
    }

    private void StartGame()
    {
        isGameActive = true;
        totalCoins = 0;
        UpdateCoinUI();
    }

    private void RestartGame()
    {
        Block[] existingBlocks = FindObjectsOfType<Block>();
        foreach (Block block in existingBlocks)
        {
            Destroy(block.gameObject);
        }

        StartGame();
    }

    public void AddCoins(int amount)
    {
        if (!isGameActive) return;

        totalCoins += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (uiManager != null)
        {
            uiManager.UpdateCoinDisplay(totalCoins);
        }
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }
}