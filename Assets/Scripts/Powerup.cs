using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _powerupSpeed = 3.0f;
    [SerializeField]
    private int powerupID;
    [SerializeField]
    AudioClip _audioClip;
    
 

    void Start()
    {
       
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        if(transform.position.y < -4.2f)
        {
            Destroy(this.gameObject);
        }   
    
    }
    private void OnTriggerEnter2D(Collider2D other)
        {

         if(other.tag == "Player")
         {
                Player player = other.transform.GetComponent<Player>();

               AudioSource.PlayClipAtPoint(_audioClip, transform.position);

                if(player != null)
                {
                    switch(powerupID)
                    {
                        case 0:
                                player.TripleShotActive();
                                break;
                        case 1:
                                player.SpeedUpActive();
                                break;
                        case 2:
                                player.ShieldsActive();
                                break;
                    }

                }

                Destroy(this.gameObject);
         }

        }

}
