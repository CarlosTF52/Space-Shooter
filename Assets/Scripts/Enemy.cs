using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField]
    private float _enemySpeed = 4.0f;

    private Player _player;

    [SerializeField]
    private GameObject _enemyLasersPrefab;
    
    Animator _animator;
    AudioSource _enemyExplosionAudioSource;

    private float _fireRate = 3.0f;
    private float _canFire = -1;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        

        _animator = gameObject.GetComponent<Animator>();
        _enemyExplosionAudioSource = GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if(Time.time > _canFire)
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
        }   
    }

      private void OnTriggerEnter2D(Collider2D other)
        {

         if(other.tag == "Player")
         {
              
                Player player = other.transform.GetComponent<Player>();

                if(player != null)
                {
                    player.Damage();
                }
                _animator.SetTrigger("OnEnemyDeath");
                _enemySpeed = 0;
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject,2.8f);
                _enemyExplosionAudioSource.Play();
         }
   
         else if(other.tag == "Laser")
         {
            Destroy(other.gameObject);
            //add 10 to score
                if(_player != null)
                {
                    _player.ScoreCount(10);
                }

            _animator.SetTrigger("OnEnemyDeath");
            _enemySpeed = 0;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
             _enemyExplosionAudioSource.Play();
         }
        }


}
