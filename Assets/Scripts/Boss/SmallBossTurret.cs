using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBossTurret : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private float _turningSpeed;

    [SerializeField]
    private float _rotationModifier;

    private float _canFire = 0;

    [SerializeField]
    private float _fireRate = 2.5f;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    [SerializeField]
    private int _turretLives = 2;

    [SerializeField]
    private GameObject _destroyed;

    [SerializeField]
    private GameObject _explosion;

    [SerializeField]
    private GameObject _turret;

    [SerializeField]
    private GameObject _damaged;

    private Vector3 _laserOffset;

    private Quaternion _laserRotation;

    private bool _inPosition;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsPlayer();
        if (_inPosition)
        {
            Fire();
        }      
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

    private void Fire()
    {
        if (Time.time > _canFire)        
        {
            _fireRate = Random.Range(2.5f, 4f);
            _canFire = Time.time + _fireRate;
            _laserOffset = new Vector3(transform.position.x, transform.position.y - 1.0f, transform.position.z);
            
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, _laserOffset, transform.rotation);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            DamageTurret();
        }
    }

    public void InPosition()
    {
        _inPosition = true;
    }

    private void DamageTurret()
    {

        if(!_inPosition) 
        {
            return;
        }

        else
        {
            _turretLives--;
            if (_turretLives < 0)
            {
                _turretLives = 0;
            }
            switch (_turretLives)
            {
                case 0:
                    _destroyed.SetActive(true);
                    _turret.SetActive(false);
                    _explosion.SetActive(true);
                    break;

                case 1:
                    _damaged.SetActive(true);

                    break;

            }
        }
        
    }

}
