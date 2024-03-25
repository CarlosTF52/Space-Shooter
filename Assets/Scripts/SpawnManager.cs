using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private IEnumerator coroutine;
    [SerializeField]
    private GameObject _enemyPrefab;
    Vector3 randomSpawn;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;


    // Start is called before the first frame update
    void Start()
    {
        coroutine = SpawnRoutine(1.0f);
        StartCoroutine(coroutine);
    }

    private IEnumerator SpawnRoutine(float waitTime)
    {
        //while loop, instantiate enemy prefab, yield wait for 5 seconds
        while(_stopSpawning == false)
        {
            
            randomSpawn = new Vector3(5.55f ,Random.Range(-9, 9), 0); //set spawn position
            GameObject newEnemy = Instantiate(_enemyPrefab, randomSpawn, Quaternion.identity);   //spawn object
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(waitTime); //wait 1 second
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
