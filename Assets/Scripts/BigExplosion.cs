using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigExplosion : MonoBehaviour
{

    AudioSource _bigExplosionAudio;

    // Start is called before the first frame update
    void Start()
    {
        _bigExplosionAudio = GetComponent<AudioSource>();
        _bigExplosionAudio.Play();
        Destroy(this.gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
