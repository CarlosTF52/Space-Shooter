
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

    private bool _colliderRefresh;

    private float _fireRate = 3.0f;
    private float _canFire = -1;

    

    // Start is called before the first frame update
    void Start()
    {

        Invoke("EnableCollider", 1f);
        this.gameObject.GetComponent<Collider2D>().enabled = false;
       _player = GameObject.Find("Player").GetComponent<Player>();
        
        _animator = gameObject.GetComponent<Animator>();
        _enemyExplosionAudioSource = GetComponent<AudioSource>();

    
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if(_colliderRefresh == true)
        {
            Invoke("EnableCollider", 1f);
            _colliderRefresh = false;
        }
        
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate; 
            GameObject enemyLaser = Instantiate(_enemyLasersPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            
            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemy();
            }
        }
    }

    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if(transform.position.y < -4.2f)
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 5.55f, 0);
            this.gameObject.GetComponent<Collider2D>().enabled = false;
            _colliderRefresh = true;
        }   
    }

    private void DestroyEnemy()
    {
        if (_player != null)
        {
            _player.ScoreCount(10);


        }
        _animator.SetTrigger("OnEnemyDeath");
        _enemySpeed = 0;
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.8f);
        _enemyExplosionAudioSource.Play();
        _canFire = 0;
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Laser")
        {

            
            Destroy(other.gameObject);
           
           

            DestroyEnemy();
            
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

    public void EnableCollider()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = true;
    }


}
