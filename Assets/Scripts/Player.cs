using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        //take current position and assign it to player character
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        //move player to the bottom if position is greater than 0 in the y position
        if(transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0 ,0);
        }
        else if(transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        //teleport player to the other side of the screen if he moves past 12 in x
        if(transform.position.x >= 12)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if(transform.position.x <= -12)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }        
    }

}
