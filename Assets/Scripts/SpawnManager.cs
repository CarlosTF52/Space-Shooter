using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    private IEnumerator _enemyCoroutine;
    private IEnumerator _powerupCoroutine;

    [SerializeField]
    private GameObject[] _enemyPrefabs;

    [SerializeField]
    private GameObject _minePrefab;

    [SerializeField]
    private GameObject[] _powerups;

    [SerializeField]
    private GameObject _bossPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private float _initialEnemyWaitTime = 3.0f;

    [SerializeField]
    private float _enemySpawnWaitTime = 1.25f;

    [SerializeField]
    private float _initialPowerupWaitTime = 2.0f;

    [SerializeField]
    private int _powerupRange = 5;

    [SerializeField]
    private Vector2 _enemySpawnRangeX = new Vector2(-9, 9);

    [SerializeField]
    private float _enemySpawnY = 5.55f;

    [SerializeField]
    private Vector2 _powerupSpawnRangeX = new Vector2(-8f, 8f);

    [SerializeField]
    private float _powerupSpawnY = 7f;

    [SerializeField]
    private Vector2 _powerupWaitTimeRange = new Vector2(3, 8);

    [SerializeField]
    private int _wavesBeforeBoss;

    private EnemyWaveManager _enemyWaveManager;

    private int _currentWave = 0;

    public bool _stopSpawning = false;

    private bool _stopSpawningPowerups = false;

    void Start()
    {
        _enemyWaveManager = GameObject.Find("EnemyWaveManager")?.GetComponent<EnemyWaveManager>();
        if (_enemyWaveManager == null)
        {
            Debug.LogError("EnemyWaveManager not found or missing component!");
        }
    }

    public void StartSpawning()
    {
        _stopSpawning = false;
        _stopSpawningPowerups = false;
        _currentWave = 0;

        StartEnemyCoroutine(_enemySpawnWaitTime);
        StartPowerupCoroutine(_initialPowerupWaitTime, _powerupRange);
    }

    private void StartEnemyCoroutine(float waitTime)
    {
        if (_enemyCoroutine != null)
        {
            StopCoroutine(_enemyCoroutine);
        }
        _enemyCoroutine = SpawnEnemyRoutine(waitTime);
        StartCoroutine(_enemyCoroutine);
    }

    private void StartPowerupCoroutine(float waitTime, int powerupRange)
    {
        if (_powerupCoroutine != null)
        {
            StopCoroutine(_powerupCoroutine);
        }
        _powerupCoroutine = SpawnPowerupRoutine(waitTime, powerupRange);
        StartCoroutine(_powerupCoroutine);
    }

    public void StopSpawningEnemies()
    {
        _stopSpawning = true;
        if (_enemyCoroutine != null)
        {
            StopCoroutine(_enemyCoroutine);
        }
    }

    public void StopSpawningPowerups()
    {
        _stopSpawningPowerups = true;
        if (_powerupCoroutine != null)
        {
            StopCoroutine(_powerupCoroutine);
        }
    }

    private IEnumerator SpawnEnemyRoutine(float waitTime)
    {
        yield return new WaitForSeconds(_initialEnemyWaitTime);
        while (!_stopSpawning)
        {
            foreach (GameObject enemyPrefab in _enemyPrefabs)
            {
                
                _enemyWaveManager?.QtyUpdate();
                Vector3 spawnPos = new Vector3(Random.Range(_enemySpawnRangeX.x, _enemySpawnRangeX.y), _enemySpawnY, 0);
                GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(waitTime);
            }

            Vector3 mineSpawnPos = new Vector3(Random.Range(_enemySpawnRangeX.x, _enemySpawnRangeX.y), _enemySpawnY, 0);
            GameObject newMine = Instantiate(_minePrefab, mineSpawnPos, Quaternion.identity);
            newMine.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(waitTime);
        }
    }

    private IEnumerator SpawnPowerupRoutine(float initialWaitTime, int powerupRange)
    {
        yield return new WaitForSeconds(initialWaitTime);
        while (!_stopSpawningPowerups)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(_powerupSpawnRangeX.x, _powerupSpawnRangeX.y), _powerupSpawnY, 0);
            int randomPowerup = Random.Range(0, _powerups.Length);
            Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(_powerupWaitTimeRange.x, _powerupWaitTimeRange.y));
        }
    }

    public void StartFinalWave()
    {
        StopSpawningEnemies();
        StopSpawningPowerups();
        
        StartCoroutine(SpawnBoss());
    }

    private IEnumerator SpawnBoss()
    {
       
        yield return new WaitForSeconds(3.0f);

        Vector3 bossSpawnPos = new Vector3(0, _enemySpawnY, 0);
        GameObject boss = Instantiate(_bossPrefab, bossSpawnPos, Quaternion.identity);
        boss.transform.parent = _enemyContainer.transform;
       
    }

    public void OnPlayerDeath()
    {
        StopSpawningEnemies();
        StopSpawningPowerups();
    }
}