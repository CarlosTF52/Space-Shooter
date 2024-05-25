using UnityEngine;

public class EnemyWaveManager : MonoBehaviour
{
    [SerializeField]
    private int _currentWave;

    [SerializeField]
    private int _maxWaves;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    

    [SerializeField]
    private int _spawnQty;

    [SerializeField]
    private int _spawnCount;

    [SerializeField]
    private int[] _maxEnemies;

    [SerializeField]
    private Player _player;

    private bool _bossIsSpawned; 

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _currentWave = 0;
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
       

        WaveUpdate();
    }

    void Update()
    {
        if (_spawnCount <= 0 && _currentWave <= _maxWaves)
        {
            _currentWave++;
            WaveUpdate();
            _spawnManager.StartSpawning();
            Debug.Log("Wave Manager Current Wave: " + _currentWave);
        }

        if (_currentWave >= _maxWaves && !_bossIsSpawned)
        {
            _currentWave = _maxWaves;
            _spawnManager.StartFinalWave();
            _bossIsSpawned = true;
            _player.UnlockPlayerMovement();
        }

        

    }

    public void WaveUpdate()
    {
        if (_currentWave <= _maxWaves)
        {
            _spawnCount = _maxEnemies[_currentWave];
            _spawnQty = _maxEnemies[_currentWave];
            _uiManager.UpdateEnemyQuantity(_spawnCount);
            _player.AddAmmo();
        }
    }

    public void QtyUpdate()
    {
        _spawnQty--;

        if (_spawnQty <= 0)
        {
            _spawnQty = 0;
            _spawnManager.StopSpawningEnemies();
            _spawnManager.StopSpawningPowerups();
        }
    }

    public void CountUpdate()
    {
        _spawnCount--;
        _uiManager.UpdateEnemyQuantity(_spawnCount);
    }
}