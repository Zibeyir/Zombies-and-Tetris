using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    public Transform SpawnPoint;
    public GameObject ZombiePrefab;

    private List<WaveData> waves;
    private int currentWave = 0;
    private int zombiesAlive = 0;
    [SerializeField] float enemyAttackDuration = 2f;
    [SerializeField] float waweDuration = 7f;


    private void Awake()
    {
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
        for (currentWave = 0; currentWave <= waves.Count; currentWave++)
        {
            WaveData wave = waves[currentWave];
            GameEvents.OnWaveStarted?.Invoke(currentWave + 1);
            zombiesAlive = wave.EnemyCount;

            for (int i = 0; i < wave.EnemyCount; i++)
            {
                SpawnZombie();
                yield return new WaitForSeconds(enemyAttackDuration); // delay between spawns
            }

            yield return new WaitForSeconds(waweDuration);
        }
       
    }

    private void SpawnZombie()
    {
        GameObject zombie = Instantiate(ZombiePrefab, new Vector3(Random.Range(6,10), SpawnPoint.position.y, SpawnPoint.position.z), SpawnPoint.rotation);
        // You could configure zombie stats here using EnemyData
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
}
