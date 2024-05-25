using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField]
    private float _zRotation = 0.5f;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, _zRotation);    
    }

    private void OnTriggerEnter2D(Collider2D other)
        {
         if(other.tag == "Laser")
         {  
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                _spawnManager.StartSpawning();
                Destroy(this.gameObject);
                
         }
        }
    
}
