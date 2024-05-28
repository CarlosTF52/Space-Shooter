using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossTurret : MonoBehaviour
{
    [SerializeField]
    private Player _player;

    [SerializeField]
    private float _turningSpeed;

    [SerializeField]
    private float _rotationModifier;

    private float _canFire = -1;

    [SerializeField]
    private float _fireRate = 3.5f;

    [SerializeField]
    private GameObject _laserBeam;

    [SerializeField]
    private GameObject _laserCharge;

    [SerializeField]
    private GameObject _damaged1;

    [SerializeField]
    private GameObject _damaged2;

    [SerializeField]
    private GameObject _destroyed;

    [SerializeField]
    private GameObject _explosion;

    [SerializeField]
    private GameObject _turret;

    private float _chargeTime;

    [SerializeField]
    private int _turretLives;

    private bool _inPosition;



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _laserBeam.SetActive(false);
        _laserCharge.SetActive(false);
        _chargeTime = 1f;
    }

    void Update()
    {
        RotateTowardsPlayer();

        if (Time.time > _canFire && _inPosition)
        {
            ChargeBeam();
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

    public void InPosition()
    {
        _inPosition = true;
    }

    private void ChargeBeam()
    {
        _laserCharge.SetActive(true);
        _chargeTime -= Time.deltaTime;

        if (_chargeTime <= 0)
        {
            _laserCharge.SetActive(false);
            FireBeam();
            _canFire = Time.time + _fireRate;
            _chargeTime = 1f;
        }
    }

    private void FireBeam()
    {
        _laserBeam.SetActive(true);
        Invoke("ResetFireBeam", 2.25f);
    }

    private void ResetFireBeam()
    {
        _laserBeam.SetActive(false);
    }

    private void DamageTurret()
    {
        if (!_inPosition)
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
                    _damaged2.SetActive(true);
                    _fireRate = 2.5f;
                    _canFire = 2.5F;
                    break;

                case 2:
                    _damaged1.SetActive(true);
                    _fireRate = 2.5f;
                    _canFire = 2.5F;
                    break;

            }
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
}
