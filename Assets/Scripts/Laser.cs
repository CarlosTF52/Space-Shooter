using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float _laserSpeed = 8.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        Vector3 direction = new Vector3(0, 1, 0);
        transform.Translate(direction * _laserSpeed * Time.deltaTime);

        if(transform.position.y > 8.0f)
        {
            Destroy(this.gameObject);
        }

    }
}
