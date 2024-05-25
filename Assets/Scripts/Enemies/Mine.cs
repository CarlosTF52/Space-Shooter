using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerObj;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private float _mineSenseDistance;

    [SerializeField]
    private float _mineSpeed;

    [SerializeField]
    private float _detectionDistance;

    AudioSource _enemyExplosionAudioSource;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private GameObject _bigExplosionPrefab;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _playerObj = _player.gameObject;   
        _enemyExplosionAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMineFollow();
    }

    private void CalculateMineFollow()
    {
        transform.Translate (Vector3.down * _mineSpeed * Time.deltaTime);

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _mineSenseDistance, transform.forward);

        if (hit.collider != null && hit.collider.name == "Player")
        {
            _playerObj = hit.collider.gameObject;
            transform.position = Vector3.MoveTowards(transform.position, _playerObj.transform.position, 0.03f);
            _detectionDistance = hit.distance;
            Debug.Log("hit player at " + _detectionDistance);
        }

        else if(hit.collider != null && hit.collider.tag == "Laser")
        {
            transform.Translate (Vector3.left * (_mineSpeed * 5) * Time.deltaTime);
        }

        if (transform.position.y < -4.2f)
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 5.55f, 0);
        }
    }

    private void DestroyMine()
    {

        if (_player != null)
        {
            _player.ScoreCount(10);
        }
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _mineSpeed = 0;
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject);
        _enemyExplosionAudioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {


            Destroy(other.gameObject);



            DestroyMine();

        }

        else if (other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            DestroyMine();

        }

        else if (other.tag == "Missiles")
        {

            DestroyMine();
            Instantiate(_bigExplosionPrefab, transform.position, Quaternion.identity);
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(other.gameObject);

        }
        else if (other.tag == "BigExplosion")
        {

            DestroyMine();

        }

    }
}
