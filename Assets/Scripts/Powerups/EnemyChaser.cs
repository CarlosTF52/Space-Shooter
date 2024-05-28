using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    [SerializeField]
    private float _senseDistance;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _detectionDistance;

    [SerializeField]
    private GameObject[] _enemy;

    [SerializeField]
    private bool[] _chasing;


    void Start()
    {

    }

    void Update()
    {
        CalculateEnemyChase();
    }

    private void CalculateEnemyChase()
    {
       

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, _senseDistance, transform.forward);

        if (!_chasing[0] || !_chasing[1] || !_chasing[2])
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }


        if (hit.collider != null && hit.collider.tag == "Enemy" && !_chasing[1] && !_chasing[2])
        {
            _enemy[0] = hit.collider.gameObject;
            transform.position = Vector3.MoveTowards(transform.position, _enemy[0].transform.position, 0.5f);
            _chasing[0] = true;


        }

        else if (hit.collider != null && hit.collider.tag == "Enemy" && !_chasing[0] && !_chasing[2])
        {
            _enemy[1] = hit.collider.gameObject;
            transform.position = Vector3.MoveTowards(transform.position, _enemy[1].transform.position, 0.5f);
            _chasing[1] = true;
        }

        else if (hit.collider != null && hit.collider.tag == "Enemy" && !_chasing[0] && !_chasing[1])
        {
            _enemy[2] = hit.collider.gameObject;
            transform.position = Vector3.MoveTowards(transform.position, _enemy[3].transform.position, 0.5f);
            _chasing[2] = true;
        }

        if (transform.position.y > 8f)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }


    }
}
