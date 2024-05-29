
using UnityEngine;

public class RedEnemy : MonoBehaviour
{

    [SerializeField]
    private float _enemySpeed = 2.0f;

    private float _canFire;

    [SerializeField]
    private bool _firing;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private GameObject _laserBeam;

    private float _fireRate = 5.0f;

    [SerializeField]
    private float _turningSpeed;

    [SerializeField]
    private float _rotationModifier;

    [SerializeField]
    private float _fireTimer = 5f;

    [SerializeField]
    private GameObject _explosionPrefab;

    private EnemyWaveManager _enemyWaveManager;

    [SerializeField]
    private GameObject _bigExplosionPrefab;

    [SerializeField]
    private GameObject _chargePrefab;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _enemyWaveManager = GameObject.Find("EnemyWaveManager").GetComponent<EnemyWaveManager>();
    }

    // Update is called once per frame
    void Update()
    {

            _fireTimer -= Time.deltaTime;
     
        
        

        if (_fireTimer <= 0)
        {
            ResetFire();
            _fireTimer = 5f;
        }
        

        if (!_firing)
        {
            CalculateMovement();
            RotateTowardsPlayer();
        }
       
        if (Time.time > _canFire && transform.position.y < _player.transform.position.y - 0.25f)
        {
            Fire();
            
        }

        

    }

    private void CalculateMovement()
    {

       transform.position = new Vector3 (transform.position.x, transform.position.y + _enemySpeed * Time.deltaTime ,transform.position.z);
        _laserBeam.SetActive(false);
        _chargePrefab.SetActive(true);
        if (transform.position.y < -4.2f)
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 5.55f, 0);
        }

    }

    private void ResetFire()
    {
        
        _firing = false;
    }

    private void Fire()
    {     
        _firing = true;
        _fireRate = Random.Range(5f, 7f);
        _canFire = Time.time + _fireRate;
        _laserBeam.SetActive(true);
        _chargePrefab.SetActive(false);

    }

    private void RotateTowardsPlayer()
    {
        if (_player != null)
        {
            Vector3 vectorToTarget = _player.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - _rotationModifier;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _turningSpeed);
        }

    }

    private void DestroyRedEnemy()
    {
        if (_player != null)
        {
            _player.ScoreCount(10);


        }
        Destroy(this.gameObject);
        _enemyWaveManager.CountUpdate();
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _enemySpeed = 0;
        Destroy(GetComponent<Collider2D>());
        _canFire = 0;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            DestroyRedEnemy();
        }


        else if (other.tag == "Missiles")
        {

            DestroyRedEnemy();
            Instantiate(_bigExplosionPrefab, transform.position, Quaternion.identity);
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(other.gameObject);

        }
        else if (other.tag == "BigExplosion")
        {

            DestroyRedEnemy();

        }

    }




}
