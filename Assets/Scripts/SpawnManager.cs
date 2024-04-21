using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private IEnumerator _enemyCoroutine;
    private IEnumerator _powerupCoroutine;
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerups;
    private Vector3 _randomEnemySpawn;

    [SerializeField]
    private GameObject _enemyContainer;



    public bool _stopSpawning = false;


    // Start is called before the first frame update
    void Start()
    {
        _enemyCoroutine = SpawnEnemyRoutine(1.0f);
        _powerupCoroutine = SpawnPowerupRoutine();
        
    }

    public void StartSpawning()
    {
        StartCoroutine(_enemyCoroutine);
        StartCoroutine(_powerupCoroutine);
    }

    

    private IEnumerator SpawnEnemyRoutine(float waitTime)
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            
            _randomEnemySpawn = new Vector3(Random.Range(-9, 9), 5.55f, 0); //set spawn position
            GameObject newEnemy = Instantiate(_enemyPrefab, _randomEnemySpawn, Quaternion.identity);   //spawn object
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(waitTime); //wait 1 second
        }
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0); //set spawn position
            int randomPowerup = Random.Range(0, 6);
            Instantiate(_powerups[randomPowerup], posToSpawn, Quaternion.identity);   //spawn object
            yield return new WaitForSeconds(Random.Range(3, 8)); //wait time
        }
        
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    

}
