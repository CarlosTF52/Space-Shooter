﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    Vector3 _laserOffset;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1.0f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        
        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is Null");
        }
    }

 
    void Update()
    {
        CalculateMovement();

        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
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
        if(transform.position.x >= 12)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if(transform.position.x <= -12)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }        
    }

    void FireLaser()
    {      
            _canFire = Time.time + _fireRate;
            _laserOffset = new Vector3(transform.position.x ,transform.position.y + 1.05f , transform.position.z);
            Instantiate(_laserPrefab, _laserOffset, Quaternion.identity);
    }

    public void Damage()
    {
        _lives--;

        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
           
            Destroy(this.gameObject);
        }

    }


}
