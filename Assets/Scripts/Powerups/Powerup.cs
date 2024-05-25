using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _powerupSpeed = 3.0f;

    [SerializeField]
    private int _powerupID;

    [SerializeField]
    AudioClip _audioClip;

    [SerializeField]
    private Player _player;

    [SerializeField]
    private float _turningSpeed;

    [SerializeField]
    private float _rotationModifier;



    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
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

    public void MoveTowardsPlayer()
    {

        Vector3 vectorToTarget = _player.transform.position - transform.position;

        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - _rotationModifier;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _turningSpeed);

        float step = _powerupSpeed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);
    }

    private void OnTriggerEnter2D(Collider2D other)
        {

         if(other.tag == "Player")
         {
                Player player = other.transform.GetComponent<Player>();

               AudioSource.PlayClipAtPoint(_audioClip, transform.position);

                if(player != null)
                {
                    switch(_powerupID)
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
                         case 3:
                                player.AmmoRecharge();
                                break;
                        case 4:
                                player.Heal();
                                break;  
                        case 5:
                                player.MissilesActive();  
                                break;
                        case 6:
                                player.SpeedDownActive();
                                break; 
                        case 7:
                                player.EnemyChaserActive();
                                break;
                    }

                }

                Destroy(this.gameObject);
         }

         if(other.tag == "Blade")
        {
            Destroy(this.gameObject);
        }

        }

}
