using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tipleShotPrefab;
    private Vector3 _laserOffset;
    private Vector3 _tripleShotOffset;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    public bool _tripleShotEnabled;
    private IEnumerator coroutine;

    private SpawnManager _spawnManager;
    

    void Start()
    {
        coroutine = PowerDownRoutine(5.0f);
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        

        
        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
        }
    }

 
    void Update()
    {
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }

    

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        //move player to the bottom if position is greater than 0 in the y position
        if(transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0 ,0);
        }
        else if(transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        //teleport player to the other side of the screen if he moves past 12 in x
        if(transform.position.x >= 9)
        {
            transform.position = new Vector3(-9, transform.position.y, 0);
        }
        else if(transform.position.x <= -9)
        {
            transform.position = new Vector3(9, transform.position.y, 0);
        }        
    }

    public void FireLaser()
    {      
            _canFire = Time.time + _fireRate;

            _laserOffset = new Vector3(transform.position.x ,transform.position.y + 1.05f , transform.position.z);
            

            if(_tripleShotEnabled == true)
            {
                _tripleShotOffset = new Vector3(transform.position.x + 0.25f, transform.position.y + 0.5f, transform.position.z);
                Instantiate(_tipleShotPrefab, _tripleShotOffset, Quaternion.identity);
            }
            else 

            Instantiate(_laserPrefab, _laserOffset, Quaternion.identity);

    }

    public void Damage()
    {
        _lives--;

        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
           
            Destroy(this.gameObject);
        }

    }

    public void TripleShotActive()
    {
        _tripleShotEnabled = true;
        
        StartCoroutine(coroutine);
    }

    private IEnumerator PowerDownRoutine(float powerTime)
    {
        //while loop, instantiate enemy prefab, yield wait for 5 seconds
        while(_tripleShotEnabled == true)
        {
            yield return new WaitForSeconds(powerTime);
            _tripleShotEnabled = false;
            
        }
    }

}
