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
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(1, 0, 0) * horizontalInput * _speed * Time.deltaTime);
        transform.Translate(new Vector3(0, 1, 0) * verticalInput * _speed * Time.deltaTime);
    }
}
