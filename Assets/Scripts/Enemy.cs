using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    [SerializeField]
    private float _enemySpeed = 4.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //move down at 4 meters per second, rendom respawn at top if gone past bottom of screen 
        
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if(transform.position.y < -4.2f)
        {
            float randomX = Random.Range(-9, 9);
            transform.position = new Vector3(randomX, 5.55f, 0);
        }   
    }

      private void OnTriggerEnter2D(Collider2D other)
        {

         if(other.tag == "Player")
         {
              
                Player player = other.transform.GetComponent<Player>();

                if(player != null)
                {
                    player.Damage();
                }

                Destroy(this.gameObject);
         }
   
         else if(other.tag == "Laser")
         {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
         }
        }


}
