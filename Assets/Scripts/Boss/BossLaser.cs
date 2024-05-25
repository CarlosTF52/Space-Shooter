using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float _laserSpeed = 8.0f;

    [SerializeField]
    private bool _isEnemyLaser;
    
    // Update is called once per frame
    void Update()
    {
      if(_isEnemyLaser == false)
      {
        MoveUp();
      }
      else
      {
        MoveDown();
      }
        

    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        if(transform.position.y > 8f)
        {

                if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

        Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

        if(transform.position.y < -5.0f)
        {

                if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

        Destroy(this.gameObject);
        }
    }

    public void AssignEnemy()
    {
              
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_isEnemyLaser == true)
        {
            other.tag = "EnemyLaser";
        }

        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            
            if(player != null)
            {
                player.Damage(); //destroy laser
                Destroy(this.gameObject);
            }
        }

    }


}
