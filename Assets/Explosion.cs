using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    AudioSource _asteroidExplosion;

    // Start is called before the first frame update
    void Start()
    {
        _asteroidExplosion = GetComponent<AudioSource>();
        _asteroidExplosion.Play();
        Destroy(this.gameObject, 3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
