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

    private bool _chasing;


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

        if (!_chasing)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
    

        if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            _enemy[0] = hit.collider.gameObject;
            transform.position = Vector3.MoveTowards(transform.position, _enemy[0].transform.position, 0.1f);
            _chasing = true;


        }

        else if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            _enemy[1] = hit.collider.gameObject;
            transform.position = Vector3.MoveTowards(transform.position, _enemy[1].transform.position, 0.03f);
            _chasing = true;
        }

        else if (hit.collider != null && hit.collider.tag == "Enemy")
        {
            _enemy[3] = hit.collider.gameObject;
            transform.position = Vector3.MoveTowards(transform.position, _enemy[3].transform.position, 0.03f);
            _chasing = true;
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
