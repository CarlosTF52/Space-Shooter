using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5.0f;

    private float _speedUpMultipler = 2;
    
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
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _shieldsVisualizer;
    private bool _tripleShotEnabled;
    private bool _speedUpEnabled;
    private bool _shieldsEnabled;
    private IEnumerator coroutine;
    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    private SpawnManager _spawnManager;

    private GameManager _gameManager;

    [SerializeField]
    private GameObject _rightEngine;

    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    AudioSource _laserAudioSource;

     [SerializeField]
    AudioSource _playerExplosionAudioSource;


 

    void Start()
    {
      
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        

        
        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is Null");
        }

        if(_gameManager == null)
        {
            Debug.LogError("The Game Manager is Null");
        }

    }

 
    void Update()
    {
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        if(Input.GetKeyDown(KeyCode.R) && _lives < 1)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            {
            Instantiate(_laserPrefab, _laserOffset, Quaternion.identity);
            }

            //play audio clip, create variable for audio clip
            _laserAudioSource.Play();


    }

    public void Damage()
    {
        //if shield is active, do nothing, deactivate shields
        if(_shieldsEnabled == true)
        {
            _shieldsEnabled = false;
            _shieldsVisualizer.SetActive(false);
            return;
            
        }
        _lives--;
        _uiManager.UpdateLives(_lives);

        switch(_lives)
        {
            case 0:
                 ResetGame();
                 break;
            
            case 1:
                _leftEngine.SetActive(true);
                break;
            
            case 2:
                _rightEngine.SetActive(true);
                break;

        }

    }

    void ResetGame()
    {
        _spawnManager.OnPlayerDeath();
        _uiManager.GameOver();
        Destroy(this.gameObject);
        _gameManager.GameOver();
        _playerExplosionAudioSource.Play();
        

    }
    public void TripleShotActive()
    {
        _tripleShotEnabled = true;
        
        StartCoroutine(PowerDownRoutine());
    }

    public void SpeedUpActive()
    {
    
        _speed = _speed * _speedUpMultipler;
        _speedUpEnabled = true;
        

        StartCoroutine(PowerDownRoutine());
    }

    public void ShieldsActive()
    {
        _shieldsEnabled = true;
       
        _shieldsVisualizer.SetActive(true);
    }

    private IEnumerator PowerDownRoutine()
    {
            yield return new WaitForSeconds(5.0f);
            _tripleShotEnabled = false;
            if(_speedUpEnabled)
            {
                _speed = _speed / _speedUpMultipler;
                _speedUpEnabled = false;    
            }
            
           
    }

    //method to add 10  to score, communicate with UI to update score
    public void ScoreCount(int points)
    {
        _score = _score + points;
        _uiManager.UpdateScore(_score);

    }

}
