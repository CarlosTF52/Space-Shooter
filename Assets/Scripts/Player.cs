using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private float _rotationSpeed;

    [SerializeField]
    private float _rotationOffset;

    private float _speedUpMultipler = 2;

    private float _speedDownSubtract = 2;

    [SerializeField]
    private GameObject _tipleShotPrefab;

    [SerializeField]
    private GameObject _missilesPrefab;

    [SerializeField]
    private GameObject _enemyChaserPrefab;

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
    private GameObject _fullShieldsVisualizer;

    [SerializeField]
    private GameObject _medShieldsVisualizer;

    [SerializeField]
    private GameObject _lowShieldsVisualizer;

    private bool _tripleShotEnabled;

    private bool _enemyChaserEnabled;

    [SerializeField]
    private bool _speedUpEnabled;

    [SerializeField]
    private bool _speedDownEnabled;

    [SerializeField]
    private bool _missilesEnabled;

    [SerializeField]
    private int _shieldPower;

    private int _ammo = 15;

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
    AudioSource _missileAudioSource;

    [SerializeField]
    AudioSource _playerExplosionAudioSource;

    [SerializeField]
    private GameObject _thrusterPrefab;

    private Vector3 _thrusterScale;
    private Vector3 _thrusterOffset;

    private float _thrustersTime;

    [SerializeField]
    private float _speedUpDuration;

    [SerializeField]
    private float _speedDownDuration;

    [SerializeField]
    private CameraControl _cameraControl;

    [SerializeField]
    private bool _tookDamage;

    [SerializeField]
    private Powerup _powerup;

    [SerializeField]
    private bool _bossPresent;

    [SerializeField]
    private GameObject _bossPrefab;

    [SerializeField]
    private GameObject _playerExplosion;

    




    void Start()
    {
        _thrusterScale = new Vector3(1, 1, 1);
        _thrusterOffset = new Vector3(0, -0.5f, 0);
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        



        if (_spawnManager == null)
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
       
        if (_tookDamage == true)
        {
            StartCoroutine(PlayerHurt());
            StartCoroutine(TookDamage());
        }


        

        if(_speedUpEnabled)
        {
            ThrustersTimerDown(_thrustersTime);
            _uiManager.UpdateThrusters(_thrustersTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammo > 0)
        {
            FireLaser();
           
        }

        if (Input.GetKey(KeyCode.C))
        {
            _powerup = FindObjectOfType<Powerup>();
            _powerup.MoveTowardsPlayer();
        }
    }

    

    void CalculateMovement()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed = _speed * _speedUpMultipler;
            
            _thrusterPrefab.transform.localScale += _thrusterScale;
            _thrusterPrefab.transform.position += _thrusterOffset;


        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _speed / _speedUpMultipler;
           
            _thrusterPrefab.transform.localScale -= _thrusterScale;
            _thrusterPrefab.transform.position -= _thrusterOffset;
        }

        


        transform.Translate(direction * _speed * Time.deltaTime);

        //move player to the bottom if position is greater than 0 in the y position
        if (!_bossPresent)
        {
            if (transform.position.y >= 0)
            {
                transform.position = new Vector3(transform.position.x, 0, 0);
            }
            else if (transform.position.y <= -3.8f)
            {
                transform.position = new Vector3(transform.position.x, -3.8f, 0);
            }
        }

        else if (_bossPresent)
        {
            _spawnManager.StopSpawningPowerups();
            if (transform.position.y >= 7.18)
            {
                transform.position = new Vector3(transform.position.x, 0, 0);
            }
            else if (transform.position.y <= -3.8f)
            {
                transform.position = new Vector3(transform.position.x, -3.8f, 0);
            }
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

    public void AddAmmo()
    {
        _ammo = _ammo + 10;
    }

    public void FireLaser()
    {     
            
            _canFire = Time.time + _fireRate;

            _laserOffset = new Vector3(transform.position.x ,transform.position.y + 1.05f , transform.position.z);

            _ammo--;


        if (_tripleShotEnabled == true)
        {
            _fireRate = 0.1f;
            _tripleShotOffset = new Vector3(transform.position.x + 0.25f, transform.position.y + 0.5f, transform.position.z);
            Instantiate(_tipleShotPrefab, _tripleShotOffset, Quaternion.identity);
            _laserAudioSource.Play();
        }
        else if (_missilesEnabled == true)
        {
            _fireRate = 1f;
            Instantiate(_missilesPrefab, transform.position, Quaternion.identity);
            _missileAudioSource.Play();
           
        }
        else if(_enemyChaserEnabled == true)
        {
            _fireRate = 0.2f;
            Instantiate(_enemyChaserPrefab, transform.position, Quaternion.identity);
            
        }
        else
        {
            _fireRate = 0.2f;
            Instantiate(_laserPrefab, _laserOffset, Quaternion.identity);
            _laserAudioSource.Play();
            
        }
        _uiManager.UpdateAmmo(_ammo);
        



    }

    public void UnlockPlayerMovement()
    {
        _bossPresent = true;
        
    }

    public void Damage()
    {

            if (_shieldPower > 0)
            {
                _shieldPower--;
                switch (_shieldPower)
                {
                    case 3:
                        _fullShieldsVisualizer.gameObject.SetActive(true);
                        _medShieldsVisualizer.gameObject.SetActive(false);
                        _lowShieldsVisualizer.gameObject.SetActive(false);
                        break;

                    case 2:
                        _fullShieldsVisualizer.gameObject.SetActive(false);
                        _medShieldsVisualizer.gameObject.SetActive(true);
                        break;

                    case 1:
                        _medShieldsVisualizer.gameObject.SetActive(false);
                        _lowShieldsVisualizer.gameObject.SetActive(true);
                        break;
                    case 0:
                        _fullShieldsVisualizer.gameObject.SetActive(false);
                        _medShieldsVisualizer.gameObject.SetActive(false);
                        _lowShieldsVisualizer.gameObject.SetActive(false);
                        break;
                }
                return;

            }


        if (_tookDamage == true)
        {
            StartCoroutine(TookDamage());
        }  
            

        if (_tookDamage == false)
        {
            
            _cameraControl.ShakeCamera(.5f, 0.25f);
            _lives--;
            _uiManager.UpdateLives(_lives);
            if (_lives < 0)
            {
                _lives = 0;
            }

            switch (_lives)
            {
                case 0:
                    ResetGame();
                    Instantiate(_playerExplosion, transform.position, Quaternion.identity);
                    break;

                case 1:
                    _leftEngine.SetActive(true);
                    break;

                case 2:
                    _rightEngine.SetActive(true);
                    break;

            }           
            _tookDamage = true;
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
        _ammo = _ammo + 5;
        _uiManager.UpdateAmmo(_ammo);

        StartCoroutine(PowerDownRoutine());
    }

    public void EnemyChaserActive()
    {
        _enemyChaserEnabled = true;
        _ammo = _ammo + 5;
        _uiManager.UpdateAmmo(_ammo);

        StartCoroutine(PowerDownRoutine());
    }

    public void SpeedUpActive()
    {
        
        _speed = _speed * _speedUpMultipler;
        _thrusterPrefab.transform.localScale += _thrusterScale;
        _thrusterPrefab.transform.position += _thrusterOffset;
        _speedUpEnabled = true;
        

        StartCoroutine(PowerDownRoutine());
    }

    public void SpeedDownActive()
    {
        _speed = _speed - _speedDownSubtract;
        _thrusterPrefab.transform.localScale -= _thrusterScale;
        _thrusterPrefab.transform.position -= _thrusterOffset;
        _speedDownEnabled = true;

        StartCoroutine(PowerDownRoutine());
    }

    public void ShieldsActive()
    {
        _shieldPower = 3;
        _fullShieldsVisualizer.gameObject.SetActive(true);
        _medShieldsVisualizer.gameObject.SetActive(false);
        _lowShieldsVisualizer.gameObject.SetActive(false);
    }

    public void AmmoRecharge()
    {
        _ammo = _ammo + 15;
        _uiManager.UpdateAmmo(_ammo);
    }

    public void Heal()
    {
        
        _lives++;
        if (_lives > 3)
        {
            _lives = 3;
        }
        _uiManager.UpdateLives(_lives);
        switch (_lives)
        {

            case 1:
                _leftEngine.SetActive(false);
                break;

            case 2:
                _rightEngine.SetActive(false);
                break;
            case 3:
                _leftEngine.SetActive(false);
                _rightEngine.SetActive(false);
                break;

        }
       
    }

    public void MissilesActive()
    {
        _missilesEnabled = true;
        _ammo = _ammo + 1;
        _uiManager.UpdateAmmo(_ammo);
        StartCoroutine(PowerDownRoutine());
    }



    private IEnumerator PowerDownRoutine()
    {
            if(_tripleShotEnabled)
            {
                yield return new WaitForSeconds(5.0f);
                _tripleShotEnabled = false;
            }
            
            if(_missilesEnabled)
            {
                yield return new WaitForSeconds(5.0f);
                _missilesEnabled = false;
            }

            if (_enemyChaserEnabled)
            {
                yield return new WaitForSeconds(5.0f);
                _enemyChaserEnabled = false;
            }
            
            else if (_speedUpEnabled)
            {
           

            yield return new WaitForSeconds(_speedUpDuration);            
                
                _speed = _speed / _speedUpMultipler;
                _thrusterPrefab.transform.localScale -= _thrusterScale;
                _thrusterPrefab.transform.position -= _thrusterOffset;
                
                _speedUpEnabled = false;
            }

            else if (_speedDownEnabled)
            {


            yield return new WaitForSeconds(_speedDownDuration);

            _speed = _speed + _speedDownSubtract;
            _thrusterPrefab.transform.localScale += _thrusterScale;
            _thrusterPrefab.transform.position += _thrusterOffset;

            _speedDownEnabled = false;
            }


    }

    private IEnumerator TookDamage()
    {
        
        while (_tookDamage == true)
        {
            

            yield return new WaitForSeconds(2.0f);
            _tookDamage = false;
               
        }
    }

    private IEnumerator PlayerHurt()
    {
        while(_tookDamage == true)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.5f);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.5f);
        }
    }

    //method to add 10  to score, communicate with UI to update score
    public void ScoreCount(int points)
    {
        
        _score = _score + points;
        _uiManager.UpdateScore(_score);

    }

    private void ThrustersTimerDown(float _thrusterAmount)
    {
        //_thrusterAmount = 100f;

        if (_speedUpEnabled)
        {
            _thrustersTime = _speedUpDuration;     
        }
        
    }

   

}
