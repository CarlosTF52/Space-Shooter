using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueEnemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private GameObject _rightBladeVisibility;

    [SerializeField]
    private GameObject _leftBladeVisibility;

    Animator _animator;

    AudioSource _enemyExplosionAudioSource;

    [SerializeField]
    private GameObject _bigExplosionPrefab;

    private float _fireRate = 3.0f;

    private float _canFire = -1;

    [SerializeField]
    private GameObject _leftBladePrefab;

    [SerializeField]
    private GameObject _rightBladePrefab;

    private Vector3 _leftRayCastOffset;

    private Vector3 _rightRayCastOffset;

    private EnemyWaveManager _enemyWaveManager;

    [SerializeField]
    private bool _enemyShieldsActive = true;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = gameObject.GetComponent<Animator>();
        _enemyExplosionAudioSource = GetComponent<AudioSource>();
        _enemyWaveManager = GameObject.Find("EnemyWaveManager").GetComponent<EnemyWaveManager>();
       
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        CalculateShots();
    }

    private void CalculateShots()
    {
        _leftRayCastOffset = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
        _rightRayCastOffset = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);

        RaycastHit2D hitLeft = Physics2D.Raycast(_leftRayCastOffset, Vector3.left, 100f);
        Debug.DrawRay(_leftRayCastOffset, Vector3.left * 10);

        RaycastHit2D hitRight = Physics2D.Raycast(_rightRayCastOffset, Vector3.right, 100f);
        Debug.DrawRay(_rightRayCastOffset, Vector3.right * 10);

        if (Time.time > _canFire && hitRight.collider != null && (hitRight.collider.name == "Player" || hitRight.collider.tag == "PowerUp") && _enemyShieldsActive == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject _rightBlade = Instantiate(_rightBladePrefab, transform.position, Quaternion.identity);
            Debug.Log("hit" + hitRight.collider.name);
        }
        else if (Time.time > _canFire && hitLeft.collider != null && (hitLeft.collider.name == "Player" || hitLeft.collider.tag == "PowerUp") && _enemyShieldsActive == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject _leftBlade = Instantiate(_leftBladePrefab, transform.position, Quaternion.identity);
            Debug.Log("hit" + hitLeft.collider.name);
        }
    }

    private void CalculateMovement()
    {
 
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y < -4.2f)
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 5.55f, 0);
        }
    }

    private void DestroyBlueEnemy()
    {
        if(_enemyShieldsActive)
        {
            _rightBladeVisibility.SetActive(false);
            _leftBladeVisibility.SetActive(false);
            _enemyShieldsActive = false;
            return;
        }
        if (_player != null)
        {
            _player.ScoreCount(10);


        }
        _enemyWaveManager.CountUpdate();
        _animator.SetTrigger("OnEnemyDeath");
        _enemySpeed = 0;
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.8f);
        _enemyExplosionAudioSource.Play();
        _canFire = 0;


    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Laser")
        {


            Destroy(other.gameObject);



            DestroyBlueEnemy();

        }

        else if (other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();

            }
            DestroyBlueEnemy();

        }


        else if (other.tag == "Missiles")
        {

            DestroyBlueEnemy();
            Instantiate(_bigExplosionPrefab, transform.position, Quaternion.identity);
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(other.gameObject);

        }
        else if (other.tag == "BigExplosion")
        {

            DestroyBlueEnemy();

        }

    }
}
