using UnityEngine;
using System.Collections.Generic;

public class GameInitializer : MonoBehaviour
{
    [Header("Core Components")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject gridManagerPrefab;
    [SerializeField] private GameObject gameManagerPrefab;
    [SerializeField] private GameObject uiManagerPrefab;

    [Header("Block Types")]
    [SerializeField] private List<BlockType> availableBlockTypes = new List<BlockType>();

    [Header("Ball Settings")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private int numberOfBalls = 3;
    [SerializeField] private Vector2 ballSpawnArea = new Vector2(5f, 5f);

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Setup Camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        SetupCamera();

        // Create and setup GridManager
        GameObject gridManagerObj = Instantiate(gridManagerPrefab);
        GridManager gridManager = gridManagerObj.GetComponent<GridManager>();

        // Create and setup GameManager
        GameObject gameManagerObj = Instantiate(gameManagerPrefab);
        GameManager gameManager = gameManagerObj.GetComponent<GameManager>();

        // Create and setup UIManager
        GameObject uiManagerObj = Instantiate(uiManagerPrefab);
        UIManager uiManager = uiManagerObj.GetComponent<UIManager>();

        // Connect components
        if (uiManager != null)
        {
            uiManager.SetupUI(availableBlockTypes);
        }

        // Spawn initial balls
        SpawnBalls();
    }

    private void SetupCamera()
    {
        //////if (mainCamera != null)
        //////{
        //////    mainCamera.orthographic = true;
        //////    mainCamera.transform.position = new Vector3(0, 0, -10);
        //////    mainCamera.transform.rotation = Quaternion.identity;
        //////    mainCamera.orthographicSize = 10f;
        //////}
    }

    private void SpawnBalls()
    {
        if (ballPrefab == null) return;

        for (int i = 0; i < numberOfBalls; i++)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-ballSpawnArea.x, ballSpawnArea.x),
                Random.Range(-ballSpawnArea.y, ballSpawnArea.y),
                0
            );

            Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        }
    }
}