using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blades : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField]
    float _bladeSpeed = 16.0f;

    BlueEnemy _blueEnemy;

 

    // Start is called before the first frame update
    void Start()
    {
        //_blueEnemy = GameObject.Find("BlueEnemy").GetComponent<BlueEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.Translate(Vector3.left * _bladeSpeed * Time.deltaTime);
    }

  

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage(); //destroy laser
                Destroy(this.gameObject);
            }
        }

    }
}
