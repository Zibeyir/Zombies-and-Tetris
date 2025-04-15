using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    public Transform SpawnPointMin;
    public Transform SpawnPointMax;

    public List<GameObject> ZombiePrefabs;

    private List<WaveData> waves;
    private int currentWave = 0;
    private int zombiesAlive = 0;

    [SerializeField] float enemyAttackDuration = 2f;
    [SerializeField] float waveDuration = 7f;

    private List<GameObject> activeZombies = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartWaves(List<WaveData> waveList)
    {
        waves = waveList;
        currentWave = 0;
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(2f);
        for (currentWave = 0; currentWave < waves.Count; currentWave++)
        {
            WaveData wave = waves[currentWave];
            GameEvents.OnWaveStarted?.Invoke(currentWave + 1);
            zombiesAlive = wave.EnemyCount;

            for (int i = 0; i < wave.EnemyCount; i++)
            {
                SpawnZombie();
                yield return new WaitForSeconds(enemyAttackDuration);
            }

            yield return new WaitForSeconds(waveDuration);
        }
    }

    private void SpawnZombie()
    {
        if (ZombiePrefabs.Count == 0)
        {
            Debug.LogWarning("No zombie prefabs assigned!");
            return;
        }

        GameObject selectedZombiePrefab = ZombiePrefabs[Random.Range(0, ZombiePrefabs.Count)];

        Vector3 spawnPos = GenerateValidSpawnPosition();

        GameObject zombie = Instantiate(selectedZombiePrefab, spawnPos, SpawnPointMax.rotation);
        activeZombies.Add(zombie);
    }

    private Vector3 GenerateValidSpawnPosition()
    {
        Vector3 spawnPos = Vector3.zero;
        bool isValidPosition = false;

        // Try to find a valid position with minimum 0.2 distance from other zombies
        while (!isValidPosition)
        {
            spawnPos = new Vector3(Random.Range(SpawnPointMin.position.x, SpawnPointMax.position.x), SpawnPointMax.position.y , Random.Range(SpawnPointMin.position.z, SpawnPointMax.position.z));

            // Check distance against all active zombies
            isValidPosition = true;
            foreach (var activeZombie in activeZombies)
            {
                if (activeZombie != null && Vector3.Distance(spawnPos, activeZombie.transform.position) < 0.2f)
                {
                    isValidPosition = false;
                    break;
                }
            }
        }

        return spawnPos;
    }

    public void OnZombieKilled()
    {
        zombiesAlive--;
        if (zombiesAlive <= 0)
        {
            GameEvents.OnWaveCompleted?.Invoke(currentWave);
            StartCoroutine(StartNextWave());
        }
    }

    public void DestroyAllZombies()
    {
        foreach (var zombie in activeZombies)
        {
            if (zombie != null)
            {
                Destroy(zombie);
            }
        }
        activeZombies.Clear();
    }

    public List<GameObject> GetActiveZombies()
    {
        return activeZombies;
    }
}
