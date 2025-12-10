using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Header("Setup")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    [Header("Wave Settings")]
    public int startingEnemies = 3;
    public int maxEnemies = 20;
    public int enemiesIncreasePerWave = 3;
    public float timeBetweenWaves = 15f;
    public float spawnCheckInterval = 2f;

    private int targetAlive;
    private float waveTimer;
    private float spawnTimer;

    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("WaveSpawner: enemyPrefab is NOT assigned!");
            return;
        }
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("WaveSpawner: No spawnPoints assigned!");
            return;
        }

        targetAlive = startingEnemies;
        Debug.Log("WaveSpawner: Starting. Target enemies = " + targetAlive);
        SpawnUpToTarget();
    }

    void Update()
    {
        waveTimer += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        // Increase target amount over time
        if (waveTimer >= timeBetweenWaves && targetAlive < maxEnemies)
        {
            targetAlive = Mathf.Min(maxEnemies, targetAlive + enemiesIncreasePerWave);
            waveTimer = 0f;
            Debug.Log("WaveSpawner: New target enemies = " + targetAlive);
        }

        // Try to fill up to targetAlive
        if (spawnTimer >= spawnCheckInterval)
        {
            SpawnUpToTarget();
            spawnTimer = 0f;
        }
    }

    void SpawnUpToTarget()
    {
        int alive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        Debug.Log("WaveSpawner: Currently alive = " + alive + " / target = " + targetAlive);

        while (alive < targetAlive)
        {
            SpawnOneEnemy();
            alive++;
        }
    }

    void SpawnOneEnemy()
    {
        Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, point.position, point.rotation);
        Debug.Log("WaveSpawner: Spawned enemy at " + point.position);
    }
}
