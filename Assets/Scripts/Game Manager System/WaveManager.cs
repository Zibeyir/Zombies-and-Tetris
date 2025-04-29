using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    public Transform SpawnPointMin;
    public Transform SpawnPointMax;

    public List<GameObject> ZombiePrefabs;

    public GameObject BossZombie;


    private List<WaveData> waves;
    private int currentWave = 0;
    private int zombiesAlive = 0;
    private float zombiesWaveUI= 0;

    WaveData wave;
    float enemyAttackDuration = 2.3f;
    [SerializeField] float waveDuration = 7f;

    public List<GameObject> activeZombies = new List<GameObject>();

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
        //StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(2f);
        if(waves[currentWave].WaveNumber >= 4)
        {
            SpawnBoss();
            Debug.Log("All waves completed!");
            yield break;
        }
        else
        {

            wave = waves[currentWave];
            //GameEvents.OnWaveStarted?.Invoke(currentWave + 1);
            //zombiesAlive = wave.EnemyCount;
            zombiesAlive = wave.EnemyCount;

            for (int i = 0; i < wave.EnemyCount; i++)
            {
                SpawnZombie();
                yield return new WaitForSeconds(enemyAttackDuration);
            }
            ++currentWave;
        }
          
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            activeZombies.Add(other.gameObject);
        }
    }
    private void SpawnBoss()
    {
        
        GameObject selectedZombiePrefab = BossZombie;

        Vector3 spawnPos = GenerateValidSpawnPosition();

        GameObject zombie = Instantiate(selectedZombiePrefab, spawnPos, SpawnPointMax.rotation);
        //activeZombies.Add(zombie);
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
        //activeZombies.Add(zombie);
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
        zombiesWaveUI = zombiesAlive;
        //Debug.Log("Wave :"+ (wave.EnemyCount - zombiesWaveUI) / wave.EnemyCount + " Zombies "+zombiesAlive+" WaveMax "+wave.EnemyCount);
        UIManager.Instance.SetWave((wave.EnemyCount - zombiesWaveUI) / wave.EnemyCount,wave.WaveNumber);
        if (zombiesAlive <= 0)
        {
            DestroyAllZombies();
            GameEvents.OnWaveCompleted?.Invoke();
            UIManager.Instance.OnEnableUpgradeScene();
            //Debug.Log("All Zombies dead");
        }
    }
    public void StartWavesAfterUpgrade()
    {
        if (waves[currentWave].WaveNumber >= 4)
        {
            SpawnBoss();
            Debug.Log("All waves completed!");

        }
        else
        {
            StartCoroutine(StartNextWave());
            enemyAttackDuration -= .7f;
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
    public void RemoveZombie(GameObject zombie)
    {
        if (activeZombies.Contains(zombie))
        {
            activeZombies.Remove(zombie);
            OnZombieKilled();
            //Destroy(zombie);
        }
    }

    public List<GameObject> GetActiveZombies()
    {
        return activeZombies;
    }
}
