using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text coinText;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Transform blockContainer; 
    [SerializeField] private GameObject draggableBlockPrefab; 

    [Header("Game Settings")]
    [SerializeField] private List<BlockType> availableBlocks = new List<BlockType>();

    [Header("Events")]
    public UnityEvent onGameStart;
    public UnityEvent onGameRestart;

    private void Start()
    {
        // Initialize UI
        ShowMenu();
        SetupButtons();
    }

    public void SetupUI(List<BlockType> blockTypes)
    {
        if (blockContainer == null || draggableBlockPrefab == null) return;

        foreach (Transform child in blockContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (BlockType blockType in blockTypes)
        {
            GameObject blockUI = Instantiate(draggableBlockPrefab, blockContainer);
            DraggableBlock draggableBlock = blockUI.GetComponent<DraggableBlock>();
            if (draggableBlock != null)
            {
            }
        }
    }

    private void SetupButtons()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
    }

    public void UpdateCoinDisplay(int coins)
    {
        if (coinText != null)
        {
            coinText.text = $"Coins: {coins}";
        }
    }

    public void ShowMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        if (gamePanel != null) gamePanel.SetActive(false);
    }

    public void ShowGame()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
        if (gamePanel != null) gamePanel.SetActive(true);
    }

    private void StartGame()
    {
        ShowGame();
        onGameStart?.Invoke();
    }

    private void RestartGame()
    {
        ShowGame();
        onGameRestart?.Invoke();
    }
}