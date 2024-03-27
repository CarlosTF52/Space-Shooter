using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _powerupSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move down at speed of 3
        transform.Translate(Vector3.down * _powerupSpeed * Time.deltaTime);
        //when we leave the screen, destroy this object
        if(transform.position.y < -4.2f)
        {
            Destroy(this.gameObject);
        }   
    
    }
        //on tigger collision
    private void OnTriggerEnter2D(Collider2D other)
        {

         if(other.tag == "Player")
         {
              
                //Player player = other.transform.GetComponent<Player>();
                Player player = other.transform.GetComponent<Player>();

                if(player != null)
                {
                    player.TripleShotActive();
                }

                Destroy(this.gameObject);
         }

        }
    //only be collectable by the player (use tags)
    //On collected destroy

}
