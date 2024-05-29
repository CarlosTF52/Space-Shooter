
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField]
    private float _enemySpeed = 4.0f;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private GameObject _enemyLasersPrefab;
    
    Animator _animator;
    AudioSource _enemyExplosionAudioSource;

    [SerializeField]
    private GameObject _bigExplosionPrefab;

    [SerializeField]
    private int _randomMove;
    private float _fireRate = 3.0f;
    private float _canFire = 2;

    private EnemyWaveManager _enemyWaveManager;


    // Start is called before the first frame update
    void Start()
    {

    
       _player = GameObject.Find("Player").GetComponent<Player>();
        _randomMove = Random.Range(0, 3);
        _animator = gameObject.GetComponent<Animator>();
        _enemyExplosionAudioSource = GetComponent<AudioSource>();
        _enemyWaveManager = GameObject.Find("EnemyWaveManager").GetComponent<EnemyWaveManager>();



    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        Fire();
        
        
    }

    private void CalculateMovement()
    {
        

        switch(_randomMove)
        {
            case 0:
                transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
                break;
            case 1:
                transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
                break;
            case 2:
                transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime);
                break;
        }
            
        if(transform.position.y < -4.2f)
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 6.55f, 0);
            _randomMove = Random.Range(0, 3);
           
           
        }

        if (transform.position.x > 10)
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 6.55f, 0);
            _randomMove = Random.Range(0, 3);
        }

        if (transform.position.x < -10)
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 6.55f, 0);
            _randomMove = Random.Range(0, 3);
        }


    }

    private void DestroyEnemy()
    {
        Destroy(GetComponent<Collider2D>());
        if (_player != null)
        {
            _player.ScoreCount(10);
        }
        Destroy(this.gameObject, 1.8f);
        _enemyWaveManager.CountUpdate();
        _animator.SetTrigger("OnEnemyDeath");
        _enemySpeed = 0;
        _enemyExplosionAudioSource.Play();
        _fireRate = 0;
    }

    private void Fire()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLasersPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemy();
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Laser")
        {
            DestroyEnemy();
            Destroy(other.gameObject);
        }

        else if (other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                
            }
            DestroyEnemy();

        }


        else if (other.tag == "Missiles")
        {
           
            DestroyEnemy();
            Instantiate(_bigExplosionPrefab, transform.position, Quaternion.identity);
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(other.gameObject);

        }
        else if (other.tag == "BigExplosion")
        {

            DestroyEnemy();

        }

    }



}
